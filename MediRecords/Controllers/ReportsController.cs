using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediRecords.Services.ReportsServices;
using MediRecords.Dto.ReportsDtos;
using MediRecords.Utility;
using System.Security.Claims;

namespace MediRecords.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly IReportsService _reportsService;

    public ReportsController(IReportsService reportsService)
    {
        _reportsService = reportsService;
    }

    [Authorize(Roles = Constant.Admin)]
    [HttpGet("clinic-kpis")]
    [ProducesResponseType(typeof(ClinicKpiReportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetClinicKpis([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
    {
        if (fromDate == default || toDate == default)
            return BadRequest(new { message = "fromDate and toDate are required." });

        if (fromDate > toDate)
            return BadRequest(new { message = "fromDate cannot be after toDate." });

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            return Unauthorized(new { message = "Invalid user token." });

        try
        {
            var result = await _reportsService.GetClinicKpiReportAsync(fromDate, toDate, userId);
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = Constant.InternalError });
        }
    }
}