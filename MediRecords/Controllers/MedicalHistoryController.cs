using System.Security.Claims;
using MediRecords.Dto.MedicalHistoryDtos;
using MediRecords.Services.MedicalHistoryServices;
using MediRecords.Utility;
using Microsoft.AspNetCore.Authorization;
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
        /// Records the Medications of the patient.
        /// </summary>
        /// <param name="patientId">The numeric ID of the patient.</param>
        [HttpPost("{patientId}/medical-history")]
        [Authorize(Roles = Constant.Physician)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddMedicalHistory(int patientId, MedicalHistoryCreateRequestDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            try {
                await _medicalHistoryService.CreateMedicalHistoryAsync(patientId, dto, userId);
                return Ok(Constant.MedicalHistoryCreated);
            }
            catch(MediRecordsException ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
