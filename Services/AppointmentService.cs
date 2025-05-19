using Hospital_OPD___Appointment_Management_System__HAMS_.Data;
using Hospital_OPD___Appointment_Management_System__HAMS_.Models;
using Hospital_OPD___Appointment_Management_System__HAMS_.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace Hospital_OPD___Appointment_Management_System__HAMS_.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly AppDbContext _context;

        public AppointmentService(AppDbContext context)
        {
            _context = context;
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
            // Optional: check doctor availability or overlapping appointments here before adding
            var doctor = await _context.Doctors.FindAsync(appointment.DoctorId);
            if (doctor == null)
            {
                throw new InvalidOperationException("Doctor not found.");
            }

            // Check if doctor is on leave
            if (doctor.IsOnLeave)
            {
                throw new InvalidOperationException("Doctor is currently on leave. Cannot book appointment.");
            }
            appointment.Status = "Scheduled"; // default status

            _context.Appointments.Add(appointment);
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
            appointment.Status = "Scheduled"; // reset status if needed
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
            // Similar to GetAppointmentsByDoctorAndDateAsync, but can include all statuses or more filters
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.DateTime.Date == date.Date)
                .Include(a => a.Patient)
                .ToListAsync();
        }


        public async Task<List<DateTime>> GetAvailableSlotsAsync(int doctorId, DateTime date)
        {
            var doctor = await _context.Doctors.FindAsync(doctorId);
            if (doctor == null || doctor.IsOnLeave)
                return new List<DateTime>(); // doctor not available or on leave

            // Define working hours
            DateTime startTime = date.Date.AddHours(8);  // 8:00 AM
            DateTime endTime = date.Date.AddHours(18);   // 6:00 PM
            DateTime breakStart = date.Date.AddHours(13); // 1:00 PM
            DateTime breakEnd = date.Date.AddHours(14);   // 2:00 PM

            List<DateTime> slots = new();

            while (startTime < endTime)
            {
                // Skip break
                if (startTime >= breakStart && startTime < breakEnd)
                {
                    startTime = breakEnd;
                    continue;
                }

                // Check if slot is already booked
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

    }
}
