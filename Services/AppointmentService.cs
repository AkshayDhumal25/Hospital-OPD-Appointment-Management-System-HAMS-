using Hospital_OPD___Appointment_Management_System__HAMS_.Data;
using Hospital_OPD___Appointment_Management_System__HAMS_.Models;
using Hospital_OPD___Appointment_Management_System__HAMS_.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using NETCore.MailKit.Core;
using IEmailService = Hospital_OPD___Appointment_Management_System__HAMS_.Services.IServices.IEmailService;
namespace Hospital_OPD___Appointment_Management_System__HAMS_.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        public AppointmentService(AppDbContext context, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Department)
                .ToListAsync();
        }

        public async Task<Appointment> GetByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Department)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        
        public async Task<Appointment> BookAppointmentAsync(Appointment appointment)
        {
            var doctor = await _context.Doctors.FindAsync(appointment.DoctorId);
            if (doctor == null || doctor.IsOnLeave)
                return null;

            if (doctor.IsOnLeave)
                return null;

            
            var slots = await GetAvailableSlotsAsync(appointment.DoctorId, appointment.DateTime.Date);

            if (slots.Count == 0)
            {
                
                DateTime nextDay = appointment.DateTime.Date.AddDays(1);
                slots = await GetAvailableSlotsAsync(appointment.DoctorId, nextDay);
                if (slots.Count == 0)
                    return null; 
                appointment.DateTime = slots.First(); 
            }
            else
            {
                
                if (!slots.Contains(appointment.DateTime))
                    appointment.DateTime = slots.First();
            }

            appointment.Status = "Scheduled";

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            var currentUser = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "Unknown User";
            var currentRole = _httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "role")?.Value ?? "Unknown Role";

           
            await LogActionAsync("Book Appointment", currentRole,
                $"Booked appointment for patient {appointment.PatientId} with doctor {appointment.DoctorId} at {appointment.DateTime} by {currentUser}");

            
            var patient = await _context.patients.FindAsync(appointment.PatientId);
            var doctor1 = await _context.Doctors.FindAsync(appointment.DoctorId);

            
            string patientEmail = patient.Email;
            string doctorName = doctor1.Name;

            
            await _emailService.SendEmailAsync(patientEmail,
                "Appointment Confirmation",
                $"Your appointment is confirmed with Doctor {doctorName} at {appointment.DateTime}");
            await _context.SaveChangesAsync();
            


            return appointment;


        }


        public async Task<bool> CancelAppointmentAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return false;

            appointment.Status = "Cancelled";
            _context.Appointments.Update(appointment);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> RescheduleAppointmentAsync(int id, DateTime newDateTime)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return false;

            appointment.DateTime = newDateTime;
            appointment.Status = "Scheduled"; 
            _context.Appointments.Update(appointment);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAndDateAsync(int doctorId, DateTime date)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.DateTime.Date == date.Date && a.Status == "Scheduled")
                .Include(a => a.Patient)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetDoctorScheduleAsync(int doctorId, DateTime date)
        {
            
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.DateTime.Date == date.Date)
                .Include(a => a.Patient)
                .ToListAsync();
        }


        public async Task<List<DateTime>> GetAvailableSlotsAsync(int doctorId, DateTime date)
        {
            var doctor = await _context.Doctors.FindAsync(doctorId);
            if (doctor == null || doctor.IsOnLeave)
                return new List<DateTime>(); 

            
            DateTime startTime = date.Date.AddHours(8);  
            DateTime endTime = date.Date.AddHours(18);   
            DateTime breakStart = date.Date.AddHours(13); 
            DateTime breakEnd = date.Date.AddHours(14);   

            List<DateTime> slots = new();

            while (startTime < endTime)
            {
                
                if (startTime >= breakStart && startTime < breakEnd)
                {
                    startTime = breakEnd;
                    continue;
                }

                
                bool isBooked = await _context.Appointments
                    .AnyAsync(a => a.DoctorId == doctorId && a.DateTime == startTime && a.Status == "Scheduled");

                if (!isBooked)
                {
                    slots.Add(startTime);
                }

                startTime = startTime.AddMinutes(30);
            }

            return slots;
        }


        private async Task LogActionAsync(string action,string performedBy, string details)
        {

            var log = new AuditLog
            {
                Action = action,
                PerformedBy = performedBy,
                PerformedAt = DateTime.Now,
                Details = details
            };

            _context.AuditLogs.Add(log);

            await _context.SaveChangesAsync();
            var currentUser = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "Unknown User";
            var currentRole = _httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "role")?.Value ?? "Unknown Role";

        }

    }
}
