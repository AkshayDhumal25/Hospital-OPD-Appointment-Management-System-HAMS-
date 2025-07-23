using Hospital_OPD___Appointment_Management_System__HAMS_.Data;
using Hospital_OPD___Appointment_Management_System__HAMS_.DTO;
using Hospital_OPD___Appointment_Management_System__HAMS_.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace Hospital_OPD___Appointment_Management_System__HAMS_.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DailyAppointmentReportDto>> GetDailyAppointmentsAsync()
        {
            var today = DateTime.Today;

            var result = await _context.Appointments
                .Where(a => a.DateTime.Date == today)
                .GroupBy(a => new
                {
                    DoctorName = a.Doctor.Name,
                    DepartmentName = a.Doctor.Department != null ? a.Doctor.Department.Name : "Unassigned"
                })

                .Select(g => new DailyAppointmentReportDto
                {
                    DoctorName = g.Key.DoctorName,
                    DepartmentName = g.Key.DepartmentName,
                    AppointmentCount = g.Count(),
                    Date = today
                })
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<DoctorUtilizationDto>> GetDoctorUtilizationAsync()
        {
            var totalSlots = 10; 

            var today = DateTime.Today;

            var result = await _context.Appointments
                .Where(a => a.DateTime.Date == today)
                .GroupBy(a => a.Doctor.Name)
                .Select(g => new DoctorUtilizationDto
                {
                    DoctorName = g.Key,
                    UtilizationPercentage = (g.Count() / (double)totalSlots) * 100
                })
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<PatientVisitFrequencyDto>> GetPatientVisitFrequenciesAsync()
        {
            var result = await _context.Appointments
                .GroupBy(a => a.Patient.Name)
                .Select(g => new PatientVisitFrequencyDto
                {
                    PatientName = g.Key,
                    VisitCount = g.Count()
                })
                .OrderByDescending(p => p.VisitCount)
                .ToListAsync();

            return result;
        }
    }

}
