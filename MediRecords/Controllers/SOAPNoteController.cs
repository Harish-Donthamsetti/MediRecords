using MediRecords.Dto.SOAPNoteDtos.Request;
using MediRecords.Dto.SOAPNoteDtos.Response;
using MediRecords.Services.SOAPNoteService;
using MediRecords.Utility;
using Microsoft.AspNetCore.Mvc;

namespace MediRecords.Controllers;

[Route("api/v1/encounters")]
[ApiController]
public class SOAPNoteController : ControllerBase
{
    private readonly ISOAPNoteService _soapNoteService;

    public SOAPNoteController(ISOAPNoteService soapNoteService)
    {
        _soapNoteService = soapNoteService;
    }

    /// <summary>
    /// Saves a SOAP note for a given encounter.
    /// Subjective tab: HPI + ROS
    /// Objective tab: ExamFindings + Observations
    /// Assessment tab: Assessment
    /// Plan tab: Plan
    /// IsDraft true = Save Draft | IsDraft false = Sign and Lock
    /// </summary>
    /// <param name="id">The Encounter ID.</param>
    /// <param name="dto">The SOAP note content.</param>
    // [Authorize(Roles = Constant.Physician)]
    [HttpPost("{id}/soap")]
    [ProducesResponseType(typeof(SOAPNoteResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SaveSOAPNote(int id, [FromBody] SaveSOAPNoteRequestDto dto)
    {
        if (id <= 0)
            return BadRequest(new { message = Constant.SOAPNoteMessages.InvalidEncounterId });

        var (success, message, data) = await _soapNoteService.SaveSOAPNoteAsync(id, dto);

        if (!success)
        {
            if (message == Constant.SOAPNoteMessages.EncounterNotFound)
                return NotFound(new { message });

            return BadRequest(new { message });
        }

        return Ok(data);
    }
}