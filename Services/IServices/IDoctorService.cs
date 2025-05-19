using Hospital_OPD___Appointment_Management_System__HAMS_.Models;

namespace Hospital_OPD___Appointment_Management_System__HAMS_.Services.IServices
{
    public interface IDoctorService
    {
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task<Doctor> GetByIdAsync(int id);
        Task<Doctor> AddAsync(Doctor doctor);
        Task<bool> UpdateAsync(Doctor doctor);
        Task<bool> DeleteAsync(int id);
        Task<bool> MarkOnLeaveAsync(int id, bool isOnLeave);
    }

}
