using MediRecords.Dto.AppointmentsDtos;
using MediRecords.Services.AppointmentsServices;
using Microsoft.AspNetCore.Mvc;

namespace MediRecords.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentsService _appointmentService;

        public AppointmentsController(IAppointmentsService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AppointmentsResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> BookAppointment([FromBody] AppointmentsRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = await _appointmentService.BookAppointmentAsync(dto);
                return CreatedAtAction(nameof(BookAppointment), new { id = response.AppointmentId }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // Unified GET endpoint with flexible filters
        [HttpGet]
        public async Task<IActionResult> GetAppointments(
            [FromQuery] int? id,
            [FromQuery] int? patientId,
            [FromQuery] int? providerId,
            [FromQuery] string? date)
        {
            var response = await _appointmentService.GetAppointmentsAsync(id, patientId, providerId, date);

            if (id.HasValue && response.Count == 0)
                return NotFound(new { message = "Appointment not found" });

            return Ok(response);
        }
    }
}
