using System.Reflection.Metadata;
using System.Security.Claims;
using MediRecords.Services.PatientServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediRecords.Utility;

namespace MediRecords.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemListController : ControllerBase
    {
        private readonly IProblemListService _problemListService;
        public ProblemListController(IProblemListService problemListService)
        {
            _problemListService = problemListService;
        }
        
        [HttpPost("{patientId}/problems")]
        [Authorize(Roles = Constant.Physician)]
        public async Task<IActionResult> AddProblem(
            int patientId,
            [FromBody] ProblemCreateRequestDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                await _problemService.CreateProblemAsync(patientId, dto, userId);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (MediRecordsException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
