using Hospital_OPD___Appointment_Management_System__HAMS_.Models;

namespace Hospital_OPD___Appointment_Management_System__HAMS_.Services.IServices
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<Appointment> GetByIdAsync(int id);
        Task<Appointment> BookAppointmentAsync(Appointment appointment);
        Task<bool> CancelAppointmentAsync(int id);
        Task<bool> RescheduleAppointmentAsync(int id, DateTime newDateTime);
        Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAndDateAsync(int doctorId, DateTime date);

        Task<IEnumerable<Appointment>> GetDoctorScheduleAsync(int doctorId, DateTime date);

        Task<List<DateTime>> GetAvailableSlotsAsync(int doctorId, DateTime date);
    }

}
