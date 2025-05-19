using Hospital_OPD___Appointment_Management_System__HAMS_.DTO;

namespace Hospital_OPD___Appointment_Management_System__HAMS_.Services.IServices
{
    public interface IReportService
    {
        Task<IEnumerable<DailyAppointmentReportDto>> GetDailyAppointmentsAsync();
        Task<IEnumerable<DoctorUtilizationDto>> GetDoctorUtilizationAsync();
        Task<IEnumerable<PatientVisitFrequencyDto>> GetPatientVisitFrequenciesAsync();
    }

}
