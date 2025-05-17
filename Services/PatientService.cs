using Hospital_OPD___Appointment_Management_System__HAMS_.Data;
using Hospital_OPD___Appointment_Management_System__HAMS_.Models;
using Hospital_OPD___Appointment_Management_System__HAMS_.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace Hospital_OPD___Appointment_Management_System__HAMS_.Services
{
    public class PatientService : IPatientService
    {
        private readonly AppDbContext _context;
        public PatientService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Patient>> GetPatients()
        {
            var patient = await _context.patients.ToListAsync();
            if(patient == null)
            {
                return null;
            }
            return patient;
        }


        public async Task<Patient> AddPatient(Patient patient)
        {
            _context.patients.Add(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task<Patient> GetPatientById(int id)
        {
            return await _context.patients.FindAsync(id);
        }

        public async Task<bool> DeletePatient(int id)
        {
            var patient = await _context.patients.FindAsync(id);
            if (patient == null)
                return false;

            _context.patients.Remove(patient);
            await _context.SaveChangesAsync();
            return true;
        }


        //public async Task<bool> UpdatePatientAsync(Patient patient)
        //{
        //    var existingPatient = await _context.patients.FindAsync(patient.Id);
        //    if (existingPatient == null)
        //        return false;

        //    // Update fields
        //    existingPatient.Name = patient.Name;
        //    existingPatient.Gender = patient.Gender;
        //    existingPatient.Email = patient.Email;
        //    existingPatient.PhoneNumber = patient.PhoneNumber;
        //    existingPatient.Address = patient.Address;
        //    existingPatient.DOB = patient.DOB;

        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        public async Task<IEnumerable<Patient>> SearchPatientsAsync(string query)
        {
            return await _context.patients
                .Where(p => p.Name.ToLower().Contains(query.ToLower())
                         || p.PhoneNumber.Contains(query)
                         || p.Email.ToLower().Contains(query.ToLower()))
                .ToListAsync();
        }


        public async Task<bool> PartialUpdatePatientAsync(Patient updatedPatient)
        {
            var existingPatient = await _context.patients.FindAsync(updatedPatient.Id);
            if (existingPatient == null)
                return false;

            // Only update properties if they have a value
            if (!string.IsNullOrWhiteSpace(updatedPatient.Name))
                existingPatient.Name = updatedPatient.Name;

            if (!string.IsNullOrWhiteSpace(updatedPatient.Gender))
                existingPatient.Gender = updatedPatient.Gender;

            if (!string.IsNullOrWhiteSpace(updatedPatient.Email))
                existingPatient.Email = updatedPatient.Email;

            if (!string.IsNullOrWhiteSpace(updatedPatient.PhoneNumber))
                existingPatient.PhoneNumber = updatedPatient.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(updatedPatient.Address))
                existingPatient.Address = updatedPatient.Address;

            if (!updatedPatient.DOB.Equals(DateTime.MinValue))
            {
                existingPatient.DOB = updatedPatient.DOB;
            }


            

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
