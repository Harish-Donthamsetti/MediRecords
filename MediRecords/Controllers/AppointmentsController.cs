using MediRecords.Domain.Enums;
using MediRecords.Dto.AppointmentDtos;
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

        [HttpPut("{id}/{status}")]
        [ProducesResponseType(typeof(AppointmentUpdateResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateAppointmentStatus(
            int id,
            AppointmentStatus status,
            [FromBody] AppointmentUpdateRequestDto request)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid appointment ID" });

            try
            {
                var response = await _appointmentService.UpdateAppointmentAsync(id, status, request);

                if (response == null)
                    return NotFound(new { message = "Appointment not found" });

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}
