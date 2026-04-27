using System.Security.Claims;
using MediRecords.Dto.ImmunizationDtos;
using MediRecords.Services.ImmunizationService;
using MediRecords.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.IdentityModel.Tokens;

namespace MediRecords.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ImmunizationController : ControllerBase
    {
        private readonly IImmunizationService _immunizationService;
        public ImmunizationController(IImmunizationService immunizationService)
        {
            _immunizationService = immunizationService;
        }

        [HttpPost("{patientId}")]
        [Authorize(Roles = Constant.Physician)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddImmunization(
            int patientId,
            [FromBody] ImmunizationCreateRequestDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                var id = await _immunizationService
                    .CreateImmunizationAsync(patientId, dto, userId);

                return Created("",
                    new ImmunizationCreateResponseDto { ImmunizationId = id }); 
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (MediRecordsException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
