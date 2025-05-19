using Hospital_OPD___Appointment_Management_System__HAMS_.DTO;
using Hospital_OPD___Appointment_Management_System__HAMS_.Models;
using Hospital_OPD___Appointment_Management_System__HAMS_.Services;
using Hospital_OPD___Appointment_Management_System__HAMS_.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital_OPD___Appointment_Management_System__HAMS_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _service;


        public AppointmentsController(IAppointmentService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "Receptionist,Admin")]
        //[Authorize]
        public async Task<IActionResult> BookAppointment([FromBody] CreateAppointmentDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid appointment data.");
           
            var appointment = new Appointment
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                DateTime = dto.DateTime,
                Status = "Scheduled" // default status
            };

            var bookedAppointment = await _service.BookAppointmentAsync(appointment);
            if (bookedAppointment == null)
                return BadRequest("Slot not available or doctor unavailable.");

            return Ok(bookedAppointment);
        }

        [HttpPatch("{id}/cancel")]
        [Authorize(Roles = "Receptionist,Admin")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var result = await _service.CancelAppointmentAsync(id);
            if (!result) return NotFound("Appointment not found.");
            return Ok("Appointment cancelled.");
        }

        [HttpPatch("{id}/reschedule")]
        [Authorize(Roles = "Receptionist,Admin")]
        public async Task<IActionResult> RescheduleAppointment(int id, [FromQuery] DateTime newDateTime)
        {
            var result = await _service.RescheduleAppointmentAsync(id, newDateTime);
            if (!result) return NotFound("Appointment not found.");
            return Ok("Appointment rescheduled.");
        }

        [HttpGet("doctor/{doctorId}")]
        [Authorize(Roles = "Receptionist,Admin,Doctor")]
        public async Task<IActionResult> GetDoctorSchedule(int doctorId, [FromQuery] DateTime date)
        {
            var schedule = await _service.GetDoctorScheduleAsync(doctorId, date);
            return Ok(schedule);
        }

        [HttpGet("doctors/{doctorId}/available-slots")]
        public async Task<IActionResult> GetAvailableSlots(int doctorId, DateTime date)
        {
            var slots = await _service.GetAvailableSlotsAsync(doctorId, date);
            if (slots.Count == 0)
                return Ok("Doctor is unavailable or on leave.");

            return Ok(slots);
        }

    }
}
