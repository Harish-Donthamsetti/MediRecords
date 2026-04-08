using MediRecords.Dto.NursingNoteDtos;
using MediRecords.Services.NursingNoteServices;
using MediRecords.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediRecords.Controllers;

[Route("api/v1")]
[ApiController]
public class NursingNoteController : ControllerBase
{
    private readonly INursingNoteService _nursingNoteService;

    public NursingNoteController(INursingNoteService nursingNoteService)
    {
        _nursingNoteService = nursingNoteService;
    }

    [HttpPost("encounters/{encounterId}/nursing-notes")]
    [ProducesResponseType(typeof(NursingNoteResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddNote(int encounterId, NursingNoteCreateRequestDto requestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var response = await _nursingNoteService.AddNoteAsync(encounterId, requestDto);
            return CreatedAtAction(nameof(AddNote), new { id = response.NursingNoteId }, response);
        }
        catch (MediRecordsException ex) when (ex.Message.Contains("closed"))
        {
            return StatusCode(409, ex.Message);
        }
        catch (MediRecordsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, Constant.InternalError);
        }
    }

    [HttpPut("nursing-notes/{noteId}")]
    [ProducesResponseType(typeof(NursingNoteResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateNote(int noteId, NursingNoteUpdateRequestDto requestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var response = await _nursingNoteService.UpdateNoteAsync(noteId, requestDto);
            return Ok(response);
        }
        catch (MediRecordsException ex) when (ex.Message.Contains("closed"))
        {
            return StatusCode(409, ex.Message);
        }
        catch (MediRecordsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, Constant.InternalError);
        }
    }
}
