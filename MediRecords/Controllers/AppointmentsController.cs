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
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Missing fields / invalid patient/provider
        [ProducesResponseType(StatusCodes.Status409Conflict)]   // Slot unavailable
        public async Task<IActionResult> BookAppointment([FromBody] AppointmentsRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                int appointmentId = await _appointmentService.BookAppointmentAsync(dto);
                return Created(string.Empty, new { AppointmentId = appointmentId });
            }
            catch (ArgumentException ex)
            {
                // Patient or Provider not found
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Slot unavailable
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
