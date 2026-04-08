using System.Security.Claims;
using MediRecords.Dto.PatientDtos;
using MediRecords.Services.PatientServices;
using MediRecords.Utility;
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
        public async Task<IActionResult> CreatePatient([FromBody] PatientCreateRequestDto requestDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                var patientId = await _patientService.CreatePatientAsync(requestDto, userId);

                return StatusCode(201, new PatientCreateResponseDto { PatientId = patientId} );
            } 
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);  // 404
            }
            catch(MediRecordsException ex)
            {
                return Conflict(ex.Message);  // 409
            }
        }
    }
}
