using Hospital_OPD___Appointment_Management_System__HAMS_.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_OPD___Appointment_Management_System__HAMS_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Receptionist")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("daily-appointments")]
        public async Task<IActionResult> GetDailyAppointments()
        {
            var result = await _reportService.GetDailyAppointmentsAsync();
            return Ok(result);
        }

        [HttpGet("doctor-utilization")]
        public async Task<IActionResult> GetDoctorUtilization()
        {
            var result = await _reportService.GetDoctorUtilizationAsync();
            return Ok(result);
        }

        [HttpGet("patient-visit-frequency")]
        public async Task<IActionResult> GetPatientVisitFrequency()
        {
            var result = await _reportService.GetPatientVisitFrequenciesAsync();
            return Ok(result);
        }
    }

}
