using System.Security.Claims;
using MediRecords.Dto.AllergyDtos;
using MediRecords.Services.AllergyServices;
using MediRecords.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediRecords.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllergyController : ControllerBase
    {
        private readonly IAllergyService _allergyService;
        public AllergyController(IAllergyService allergyService)
        {
            _allergyService = allergyService;
        }

        /// <summary>
        /// Enters the allergy of the patient.
        /// </summary>
        /// <param name="patientId">The numeric ID of the patient.</param>
        [HttpPost("{patientId}/allergies")]
        [Authorize(Roles = Constant.Physician)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAllergy(int patientId, AllergyCreateRequestDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                await _allergyService.CreateAllergyAsync(patientId, dto, userId);
                return Ok(Constant.AllergyCreated);
            }
            catch (MediRecordsException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
