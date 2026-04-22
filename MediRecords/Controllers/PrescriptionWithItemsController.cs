using MediRecords.Dto.PrescriptionWithItemsDtos;
using MediRecords.Services.PrescriptionWithItemsServices;
using MediRecords.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediRecords.Domain.Entities;
using MediRecords.Repository.PrescriptionWithItemsRepository;

namespace MediRecords.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PrescriptionWithItemsController : ControllerBase
{
    private readonly IPrescriptionWithItemsService _service;

    public PrescriptionWithItemsController(MediRecordsDbContext context)
    {
        var repository = new MediRecords.Repository.PrescriptionWithItemsRepository.PrescriptionWithItemsRepository(context);
        _service = new PrescriptionWithItemsService(repository);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(PrescriptionWithItemsResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePrescriptionWithItems([FromBody] CreatePrescriptionWithItemsRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var response = await _service.CreatePrescriptionWithItemsAsync(request);
            return CreatedAtAction(nameof(GetPrescriptionWithItemsById), new { prescriptionId = response.PrescriptionId }, response);
        }
        catch (MediRecordsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, Constant.InternalError);
        }
    }

    [HttpGet("{prescriptionId}")]
    [Authorize]
    [ProducesResponseType(typeof(PrescriptionWithItemsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPrescriptionWithItemsById(int prescriptionId)
    {
        try
        {
            var response = await _service.GetPrescriptionWithItemsByIdAsync(prescriptionId);
            if (response == null)
                return NotFound("Prescription not found.");

            return Ok(response);
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

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<PrescriptionWithItemsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllPrescriptionsWithItems()
    {
        try
        {
            var response = await _service.GetAllPrescriptionsWithItemsAsync();
            return Ok(response);
        }
        catch (Exception)
        {
            return StatusCode(500, Constant.InternalError);
        }
    }

    [HttpPut("{prescriptionId}")]
    [Authorize]
    [ProducesResponseType(typeof(PrescriptionWithItemsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePrescriptionWithItems(int prescriptionId, [FromBody] UpdatePrescriptionWithItemsRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var response = await _service.UpdatePrescriptionWithItemsAsync(prescriptionId, request);
            return Ok(response);
        }
        catch (MediRecordsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, Constant.InternalError);
        }
    }

    [HttpDelete("{prescriptionId}")]
    [Authorize]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletePrescriptionWithItems(int prescriptionId)
    {
        try
        {
            var result = await _service.DeletePrescriptionWithItemsAsync(prescriptionId);
            return Ok(result);
        }
        catch (MediRecordsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, Constant.InternalError);
        }
    }
}
