using Hospital_OPD___Appointment_Management_System__HAMS_.Data;
using Hospital_OPD___Appointment_Management_System__HAMS_.Models;
using Hospital_OPD___Appointment_Management_System__HAMS_.Services;
using Hospital_OPD___Appointment_Management_System__HAMS_.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital_OPD___Appointment_Management_System__HAMS_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {

        private readonly AppDbContext _context;
        private IPatientService _patientService;

        public PatientController(IPatientService patientService, AppDbContext context)
        {
            _context = context;
            _patientService = patientService;
        }


        [HttpGet]
        public async Task<IActionResult> GetPatient()
        {
            var patient = await _patientService.GetPatients();
            return Ok(patient);
        }



        [HttpPost]
        public async Task<IActionResult> PostPatient(Patient patient)
        {
            var addedPatient = await _patientService.AddPatient(patient);
            return Ok(addedPatient);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var patient = await _patientService.GetPatientById(id);
            if (patient == null)
                return NotFound();

            return Ok(patient);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var deleted = await _patientService.DeletePatient(id);
            if (!deleted)
                return NotFound();

            return Ok(new { message = "Patient deleted successfully" });
        }


        //[HttpPut("{id}")]
        ////[Authorize(Roles = "Receptionist,Admin")]
        //public async Task<IActionResult> UpdatePatient(int id, [FromBody] Patient patient)
        //{
        //    if (id != patient.Id)
        //        return BadRequest("Patient ID mismatch.");

        //    var existingPatient = await _context.patients.FindAsync(id);
        //    if (existingPatient == null)
        //        return NotFound("Patient not found.");

        //    // Update properties
        //    existingPatient.Name = patient.Name;
        //    existingPatient.Gender = patient.Gender;
        //    existingPatient.Email = patient.Email;
        //    existingPatient.PhoneNumber = patient.PhoneNumber;
        //    existingPatient.Address = patient.Address;
        //    existingPatient.DOB = patient.DOB;

        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        [HttpGet("search")]
        //[Authorize(Roles = "Receptionist,Admin")]
        public async Task<IActionResult> SearchPatients(string query)
        {
            var patients = await _patientService.SearchPatientsAsync(query);
            if (patients == null || !patients.Any())
                return NotFound("No matching patients found.");

            return Ok(patients);
        }


        [HttpPut("{id}")]
        //[Authorize(Roles = "Receptionist,Admin")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] Patient patient)
        {
            if (id != patient.Id)
                return BadRequest("Patient ID mismatch.");

            var updated = await _patientService.PartialUpdatePatientAsync(patient);
            if (!updated)
                return NotFound("Patient not found.");

            return NoContent();
        }

    }
}
