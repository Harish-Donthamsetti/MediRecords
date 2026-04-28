using Microsoft.AspNetCore.Mvc;
using MediRecords.Dto.DocumentDtos;
using MediRecords.Services.DocumentServices;
using MediRecords.Utility;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MediRecords.Domain.Enums;

namespace MediRecords.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class DocumentController : ControllerBase
{
    private readonly IDocumentService _service;

    public DocumentController(IDocumentService service)
    {
        _service = service;
    }

    /// <summary>
    /// Upload a document for a patient
    /// </summary>
    /// <param name="request">Document upload request containing file and metadata</param>
    /// <returns>Document upload response with document details</returns>
    [HttpPost("upload")]
    [Authorize(Roles = nameof(UserRoleEnums.Admin) + "," + nameof(UserRoleEnums.Physician) + "," + nameof(UserRoleEnums.Nurse) + "," + nameof(UserRoleEnums.FrontDesk))]
    [ProducesResponseType(typeof(DocumentUploadResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadDocument([FromForm] DocumentUploadRequestDto request)
    {
        try
        {
            // Get current user ID from JWT claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized(new { message = "Invalid or missing user ID" });

            // Check user roles - only Doctor, Nurse, FrontDesk, Admin can upload
            var userRoles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            var allowedRoles = new[] { "Doctor", "Nurse", "FrontDesk", "Admin" };

            if (!userRoles.Any(role => allowedRoles.Contains(role)))
                return Forbid();

            var result = await _service.UploadDocumentAsync(request, userId);
            return Ok(result);
        }
        catch (MediRecordsException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
        }
    }
}
