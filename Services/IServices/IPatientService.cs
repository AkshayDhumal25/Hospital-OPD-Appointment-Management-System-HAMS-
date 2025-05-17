using Hospital_OPD___Appointment_Management_System__HAMS_.Models;

namespace Hospital_OPD___Appointment_Management_System__HAMS_.Services.IServices
{
    public interface IPatientService
    {
        public Task<List<Patient>> GetPatients();

        public Task<Patient> AddPatient(Patient patient);
        public Task<Patient> GetPatientById(int id);
        public Task<bool> DeletePatient(int id);

        public Task<bool> PartialUpdatePatientAsync(Patient patient);
        public Task<IEnumerable<Patient>> SearchPatientsAsync(string query);

        
    }
}
