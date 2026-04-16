using System.Security.Claims;
using MediRecords.Dto.MedicalHistoryDtos;
using MediRecords.Services.MedicalHistoryServices;
using MediRecords.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace MediRecords.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalHistoryController : ControllerBase
    {
        private readonly IMedicalHistoryService _medicalHistoryService;
        public MedicalHistoryController(IMedicalHistoryService medicalHistoryService)
        {
            _medicalHistoryService = medicalHistoryService;
        }

        /// <summary>
        /// Enters the MedicalHistory of the patient.
        /// </summary>
        /// <param name="patientId">The numeric ID of the patient.</param>
        [HttpPost("{patientId}/medical-history")]
        public async Task<IActionResult> AddMedicalHistory(int patientId, MedicalHistoryCreateRequestDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            try {
                await _medicalHistoryService.CreateMedicalHistoryAsync(patientId, dto, userId);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch(MediRecordsException ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
