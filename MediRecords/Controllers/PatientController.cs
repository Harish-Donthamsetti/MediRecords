using System.Security.Claims;
using MediRecords.Domain.Entities;
using MediRecords.Dto.PatientDtos;
using MediRecords.Services.PatientServices;
using MediRecords.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediRecords.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreatePatient([FromBody] PatientCreateRequestDto requestDto)
        {
            if(!ModelState.IsValid || requestDto == null)
            {
                return BadRequest(ModelState);
            }
            
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            try
            {
                var userId = int.Parse(userIdClaim.Value);

                var patientId = await _patientService.CreatePatientAsync(requestDto, userId);

                return Created("", new PatientCreateResponseDto { PatientId = patientId });
            } 
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(MediRecordsException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
