using MediRecords.Dto.CarePlanDtos.Request;
using MediRecords.Dto.CarePlanDtos.Response;
using MediRecords.Services.CarePlanServices;
using MediRecords.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediRecords.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CarePlanController : ControllerBase
{
    private readonly ICarePlanService _carePlanService;

    public CarePlanController(ICarePlanService carePlanService)
    {
        _carePlanService = carePlanService;
    }

    /// <summary>
    /// Creates a new care plan for a patient with goals and instructions.
    /// Goals are passed as a list and stored as JSON internally.
    /// Status: false = Active | true = Completed
    /// </summary>
    [Authorize(Roles = Constant.Physician)]
    [HttpPost]
    [ProducesResponseType(typeof(CarePlanResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCarePlan([FromBody] CreateCarePlanRequestDto dto)
    {
        var (success, message, data) = await _carePlanService.CreateCarePlanAsync(dto);

        if (!success)
        {
            if (message == Constant.CarePlanMessages.PatientNotFound)
                return NotFound(new { message });

            return BadRequest(new { message });
        }

        // 201 Created with CarePlanId as per acceptance criteria
        return CreatedAtAction(nameof(CreateCarePlan),
            new { id = data!.CarePlanId }, data);
    }
}