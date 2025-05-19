using Hospital_OPD___Appointment_Management_System__HAMS_.DTO;
using Hospital_OPD___Appointment_Management_System__HAMS_.Models;
using Hospital_OPD___Appointment_Management_System__HAMS_.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_OPD___Appointment_Management_System__HAMS_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var doctors = await _doctorService.GetAllAsync();
            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);
            if (doctor == null) return NotFound();
            return Ok(doctor);
        }

        //[HttpPost]
        ////[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Add([FromBody] Doctor doctor)
        //{
        //    var created = await _doctorService.AddAsync(doctor);
        //    return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        //}
        [HttpPost]
        public async Task<IActionResult> Add(CreateDoctorDto dto)
        {
            var doctor = new Doctor
            {
                Name = dto.Name,
                DepartmentId = dto.DepartmentId,
                Specialization = dto.Specialization,
                Availability = dto.Availability,
                IsOnLeave = dto.IsOnLeave
            };

            await _doctorService.AddAsync(doctor);
            return Ok(doctor);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Update(int id, [FromBody] Doctor doctor)
        {
            if (id != doctor.Id) return BadRequest("Id mismatch.");
            var updated = await _doctorService.UpdateAsync(doctor);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _doctorService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id}/leave")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MarkOnLeave(int id, [FromQuery] bool isOnLeave)
        {
            var result = await _doctorService.MarkOnLeaveAsync(id, isOnLeave);
            if (!result) return NotFound();
            return NoContent();
        }
    }

}


