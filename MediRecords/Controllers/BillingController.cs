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
}