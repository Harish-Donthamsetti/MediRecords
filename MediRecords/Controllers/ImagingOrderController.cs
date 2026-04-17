using MediRecords.Domain.Entities;
using MediRecords.Dto.ImagingOrderDto;
using MediRecords.Services.ImagingOrderServices;
using MediRecords.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediRecords.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagingOrderController : ControllerBase
    {
        private readonly IImagingOrderServices _service;

        public ImagingOrderController(IImagingOrderServices service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = Constant.Physician)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> CreateAsync([FromBody] ImagingOrderRequestDto dto)
        {
            if (dto == null)
                return BadRequest(Constant.RequestCannotBeNull);

            try
            {
                await _service.AddAsync(dto);
                return StatusCode(StatusCodes.Status201Created, Constant.OrderCreated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constant.InternalServerError);
            }
        }
        [HttpGet]
        [Authorize(Roles = Constant.Physician)]
        [ProducesResponseType(typeof(List<ImagingOrder>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllAsync([FromQuery] ImagingOrderFilterDto filter)
        {
            try
            {
                var results = await _service.GetAllAsync(filter);
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constant.InternalServerError);
            }
        }
    }
}
