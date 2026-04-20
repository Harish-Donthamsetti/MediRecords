using MediRecords.Dto.PrescriptionItemDtos;
using MediRecords.Services.PrescriptionItemServices;
using MediRecords.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediRecords.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PrescriptionItemController : ControllerBase
{
    private readonly IPrescriptionItemService _prescriptionItemService;

    public PrescriptionItemController(IPrescriptionItemService prescriptionItemService)
    {
        _prescriptionItemService = prescriptionItemService;
    }

    /// <summary>
    /// Retrieves all prescription items.
    /// </summary>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<PrescriptionItemResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllPrescriptionItems()
    {
        try
        {
            var result = await _prescriptionItemService.GetAllPrescriptionItemsAsync();
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, Constant.InternalError);
        }
    }

    /// <summary>
    /// Retrieves a specific prescription item by ID.
    /// </summary>
    [HttpGet("{itemId}")]
    [Authorize]
    [ProducesResponseType(typeof(PrescriptionItemResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPrescriptionItemById(int itemId)
    {
        try
        {
            var result = await _prescriptionItemService.GetPrescriptionItemByIdAsync(itemId);
            if (result == null)
            {
                return NotFound("Prescription item not found.");
            }
            return Ok(result);
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

    /// <summary>
    /// Creates a new prescription item.
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(PrescriptionItemResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePrescriptionItem([FromBody] PrescriptionItemRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _prescriptionItemService.CreatePrescriptionItemAsync(request);
            return CreatedAtAction(nameof(GetPrescriptionItemById), new { itemId = result.ItemId }, result);
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

    /// <summary>
    /// Updates an existing prescription item.
    /// </summary>
    [HttpPut("{itemId}")]
    [Authorize]
    [ProducesResponseType(typeof(PrescriptionItemResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePrescriptionItem(int itemId, [FromBody] PrescriptionItemRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _prescriptionItemService.UpdatePrescriptionItemAsync(itemId, request);
            return Ok(result);
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

    /// <summary>
    /// Deletes a prescription item by ID.
    /// </summary>
    [HttpDelete("{itemId}")]
    [Authorize]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletePrescriptionItem(int itemId)
    {
        try
        {
            var result = await _prescriptionItemService.DeletePrescriptionItemAsync(itemId);
            return Ok(result);
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