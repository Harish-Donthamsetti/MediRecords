using System;

namespace MediRecords.Dto.UserDtos;

public class UserResponseDto
{
        public int UserId { get; set; }
        public required string Name { get; set; } 

        public required int RoleId { get; set; }

        public string? Phone { get; set; } = null!;

        public bool Status { get; set; } = true;
}
