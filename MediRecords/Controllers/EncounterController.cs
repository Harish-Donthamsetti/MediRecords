using Microsoft.AspNetCore.Http;
using MediRecords.Utility;
using MediRecords.Services.EncounterServices;
using Microsoft.AspNetCore.Mvc;
using MediRecords.Dto.EncounterDtos.Response;
using MediRecords.Dto.EncounterDtos.Request;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Authorization;

namespace MediRecords.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EncounterController : ControllerBase
    {
        private readonly IEncounterService _encounterService;

        public EncounterController(IEncounterService encounterService)
        {
            _encounterService = encounterService;
        }

        [HttpGet("workspace")]
        [Authorize(Roles = Constant.Physician)]
        [ProducesResponseType(typeof(IEnumerable<EncounterSummaryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetWorkspace([FromQuery] int providerId, [FromQuery] DateTime? date)
        {
            if (providerId <= 0)
                return BadRequest(new { message = Constant.EncounterMessages.InvalidProviderId });

            try
            {
                var result = await _encounterService.GetWorkspaceAsync(providerId, date);
                return Ok(result);
            }
            catch (MediRecordsException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = Constant.EncounterMessages.SomethingWentWrong });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Constant.Physician)]
        [ProducesResponseType(typeof(EncounterDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = Constant.EncounterMessages.InvalidEncounterId });

            try
            {
                var result = await _encounterService.GetEncounterByIdAsync(id);

                if (result == null)
                    return NotFound(new { message = Constant.EncounterMessages.EncounterNotFound });

                return Ok(result);
            }
            catch (MediRecordsException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = Constant.EncounterMessages.SomethingWentWrong });
            }
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = Constant.Physician)]
        [ProducesResponseType(typeof(EncounterStatusResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] EncounterStatusUpdateDto dto)
        {
            if (id <= 0)
                return BadRequest(new { message = Constant.EncounterMessages.InvalidEncounterId });

            try
            {
                var (success, message, data) = await _encounterService.UpdateEncounterStatusAsync(id, dto);

                if (!success)
                {
                    // 404 if encounter not found, 400 for all other failures
                    if (message == Constant.EncounterMessages.EncounterNotFound)
                        return NotFound(new { message });

                    return BadRequest(new { message });
                }

                return Ok(data);
            }
            catch (MediRecordsException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = Constant.EncounterMessages.SomethingWentWrong });
            }
        }
    }
}
