using Microsoft.AspNetCore.Mvc;
using MediRecords.Services.PrescriptionService;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using MediRecords.Dto.PrescriptionDtos;
using MediRecords.Utility;
namespace MediRecords.Controllers;

[Route("api/v1")]
[ApiController]

public class PrescriptionController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;

    public PrescriptionController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }

    [HttpPost("[controller]/{EncounterId}/create")]
    [ProducesResponseType(typeof(PrescriptionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePrescription([FromRoute] int EncounterId, [FromBody] CreatePrescriptionRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var response = await _prescriptionService.CreatePrescriptionAsync(EncounterId, request);
            return CreatedAtAction(nameof(CreatePrescription), new { id = response.PrescriptionId }, response);
        }
        catch (MediRecordsException ex) when (ex.Message.Contains("conflict"))
        {
            return StatusCode(409, ex.Message);
        }
        catch (MediRecordsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while creating the prescription.");
        }
    }

    [HttpGet("[controller]s")]
    [ProducesResponseType(typeof(IEnumerable<PrescriptionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllPrescriptions()
    {
        try
        {
            var response = await _prescriptionService.GetAllPrescriptionsAsync();
            return Ok(response);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching prescriptions.");
        }
    }

    [HttpGet("[controller]/{PrescriptionId}")]
    [ProducesResponseType(typeof(PrescriptionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPrescriptionById([FromRoute] int PrescriptionId)
    {
        try
        {
            var response = await _prescriptionService.GetPrescriptionByIdAsync(PrescriptionId);
            return Ok(response);
        }
        catch (MediRecordsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching the prescription.");
        }
    }

    [HttpPut("[controller]/{PrescriptionId}/update")]
    [ProducesResponseType(typeof(PrescriptionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePrescription([FromRoute] int PrescriptionId, [FromBody] UpdatePrescriptionRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var response = await _prescriptionService.UpdatePrescriptionAsync(PrescriptionId,request);
            return Ok(response);
        }
        catch (MediRecordsException ex) when (ex.Message.Contains("conflict"))
        {
            return StatusCode(409, ex.Message);
        }
        catch (MediRecordsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while updating the prescription.");
        }
    }

    [HttpDelete("[controller]/{PrescriptionId}/delete")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletePrescription([FromRoute] int PrescriptionId)
    {
        try
        {
            var response = await _prescriptionService.DeletePrescriptionAsync(PrescriptionId);
            return Ok(response);
        }
        catch (MediRecordsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while deleting the prescription.");
        }
    }
}