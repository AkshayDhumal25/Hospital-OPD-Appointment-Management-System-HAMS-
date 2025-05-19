using Hospital_OPD___Appointment_Management_System__HAMS_.Models;

namespace Hospital_OPD___Appointment_Management_System__HAMS_.Services.IServices
{
    public interface IMedicalRecordService
    {
        Task<IEnumerable<MedicalRecord>> GetAllAsync();
        Task<MedicalRecord> GetByIdAsync(int id);
        Task<IEnumerable<MedicalRecord>> GetByPatientIdAsync(int patientId);
        Task<MedicalRecord> AddAsync(MedicalRecord record);
    }

}
