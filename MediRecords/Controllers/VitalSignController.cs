using System.Net;
using MediRecords.Dto.VitalSignDtos;
using MediRecords.Services.VitalSignServices;
using MediRecords.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediRecords.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class VitalSignController : ControllerBase
{
    private readonly IVitalSignService _vitalSignService;

    public VitalSignController(IVitalSignService vitalSignService)
    {
        _vitalSignService = vitalSignService;
    }

    /// <summary>
    /// Captures vital signs for an encounter, automatically calculates BMI if height and weight are provided.
    /// </summary>
    /// <param name="requestDto">The vital signs data transfer object.</param>
    /// <returns>201 Created with VitalId</returns>
    [HttpPost("capture")]
    // [Authorize] // Commented out to allow access without a token for now
    [ProducesResponseType(typeof(VitalSignCreateResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CaptureVitals(VitalSignCreateRequestDto requestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var response = await _vitalSignService.CreateVitalSignsAsync(requestDto);
            return CreatedAtAction(nameof(CaptureVitals), new { id = response.VitalId }, response);
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