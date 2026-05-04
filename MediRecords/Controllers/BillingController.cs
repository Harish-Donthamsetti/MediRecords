using MediRecords.Dto.BillingDtos.Request;
using MediRecords.Dto.BillingDtos.Response;
using MediRecords.Services.BillingServices;
using MediRecords.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediRecords.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BillingController : ControllerBase
{
    private readonly IBillingService _billingService;

    public BillingController(IBillingService billingService)
    {
        _billingService = billingService;
    }

    [Authorize(Roles = $"{Constant.Physician},{Constant.Admin}")]
    [HttpPost("visit-charges")]
    [ProducesResponseType(typeof(VisitChargeResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AssignVisitCharge([FromBody] AssignVisitChargeRequestDto dto)
    {
        var (success, message, data, statusCode) =
            await _billingService.AssignVisitChargeAsync(dto);

        return statusCode switch
        {
            201 => CreatedAtAction(nameof(AssignVisitCharge),
                       new { id = data!.ChargeId }, data),
            404 => NotFound(new { message }),
            409 => Conflict(new { message }),
            500 => StatusCode(500, new { message }),
            _   => BadRequest(new { message })
        };
    }

    [Authorize(Roles = Constant.Admin)]
    [HttpPut("visit-charges/{id}")]
    [ProducesResponseType(typeof(VisitChargeResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateVisitChargeAmount(
        int id, [FromBody] UpdateVisitChargeRequestDto dto)
    {
        var (success, message, data, statusCode) =
            await _billingService.UpdateVisitChargeAmountAsync(id, dto);

        return statusCode switch
        {
            200 => Ok(data),
            404 => NotFound(new { message }),
            500 => StatusCode(500, new { message }),
            _   => BadRequest(new { message })
        };
    }

    [Authorize(Roles = $"{Constant.Physician},{Constant.Admin}")]
    [HttpGet("encounters/{id}/charges")]
    [ProducesResponseType(typeof(IEnumerable<VisitChargeResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetChargesByEncounterId(int id)
    {
        if (id <= 0)
            return BadRequest(new { message = Constant.BillingMessages.InvalidEncounterId });

        var (success, message, data, statusCode) =
            await _billingService.GetChargesByEncounterIdAsync(id);

        return statusCode switch
        {
            200 => Ok(data),
            404 => NotFound(new { message }),
            500 => StatusCode(500, new { message }),
            _   => BadRequest(new { message })
        };
    }

    [Authorize(Roles = $"{Constant.Admin},{Constant.FrontDesk}")]
    [HttpGet("unbilled-encounters")]
    [ProducesResponseType(typeof(PagedResponseDto<UnbilledEncounterResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUnbilledEncounters(
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] int? providerId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var (success, message, data, statusCode) =
            await _billingService.GetUnbilledEncountersAsync(
                fromDate, toDate, providerId, page, pageSize);

        return statusCode switch
        {
            200 => Ok(data),
            500 => StatusCode(500, new { message }),
            _   => BadRequest(new { message })
        };
    }
}