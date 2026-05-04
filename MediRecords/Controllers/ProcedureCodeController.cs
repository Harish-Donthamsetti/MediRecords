using System.Reflection.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediRecords.Utility;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using MediRecords.Services.ProcedureCodeServices;
using MediRecords.Dto.ProcedureCodeDtos;

namespace MediRecords.Controllers
{
    [Route("api/v1/billing/[controller]")]
    [ApiController]
    public class ProcedureCodeController : ControllerBase
    {
        private readonly IProcedureCodeService _service;

        public ProcedureCodeController(IProcedureCodeService service)
        {
            _service = service;
        }
        /// <summary>
        /// this is used to pass the data to the service layer
        /// </summary>
        /// <param name="dto">data that has to be inserted</param>
        /// <returns></returns>
        [HttpPost]
        // [Authorize(Roles = Utility.Constant.Admin)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateProcedureCode(ProcedureCodeRequestDto dto)
        {
            try
            {
                await _service.AddAsync(dto);
                return StatusCode(StatusCodes.Status201Created, Utility.Constant.ProblemCreated);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Utility.Constant.InternalServerError);
            }
        }

        /// <summary>
        /// this route is for getting all the procedure code
        /// </summary>
        /// <returns>this will return all the procedure code</returns>
        [HttpGet]
        // [Authorize(Roles = $"{Utility.Constant.Physician},{Utility.Constant.Admin}")]
        [ProducesResponseType(typeof(IEnumerable<ProcedureCodeViewDtos>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllProcedure([FromQuery] string? filterCode)
        {
            try
            {
                var procedures = await _service.GetAllProcedure(filterCode ?? string.Empty);
                return Ok(procedures);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Utility.Constant.InternalError);
            }
        }

        /// <summary>
        /// this route is for update the details of procedure code
        /// </summary>
        /// <param name="CodeId">this is for updating the data for this CodeId</param>
        /// <param name="dto">data that has to be updated</param>
        /// <returns></returns>
        [HttpPatch("{CodeId}")]
        [Authorize(Roles = Utility.Constant.Admin)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string),StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ProcedureCodeUpdate(int CodeId, ProcedureCodeUpdateDtos dto)
        {
            try
            {
                await _service.UpdateProcedureAsync(CodeId, dto);
                return Ok(Utility.Constant.ProcdureUpdated);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, Utility.Constant.InternalServerError);
            }
        }
    }
}
