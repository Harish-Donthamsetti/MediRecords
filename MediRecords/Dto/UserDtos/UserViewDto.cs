using System;

namespace MediRecords.Dto.UserDtos;

/// <summary>
/// Represents a read-only view of a user's information for API responses.
/// This DTO hides sensitive data like passwords and internal database flags.
/// </summary>
public class UserViewDto
{
    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    public required int UserId { get; set; }

    /// <summary>
    /// The full name of the user.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The primary contact email address.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// The user's contact phone number (optional).
    /// </summary>
    public string? Phone { get; set; }
    
    /// <summary>
    /// The display name of the user's assigned role (e.g., "Admin", "User").
    /// </summary>
    public required string RoleName { get; set; }
}