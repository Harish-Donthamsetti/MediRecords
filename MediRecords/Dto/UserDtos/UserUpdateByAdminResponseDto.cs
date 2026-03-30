using System;
using MediRecords.Domain.Entities;
using MediRecords.Domain.Enums;
namespace MediRecords.Dto.UserDtos
{
    public class UserUpdateByAdminResponseDto
    {
        public int UserID { get; init; }
        public string Name { get; set; }
        public int RoleID { get; set; }
        public string Email { get; init; }
        public string Phone { get; set; }
        public bool Status { get; set; }
    }

    public static class UserUpdateByAdminResponseExtension
    {
        public static UserUpdateByAdminResponseDto ToUserUpdateByAdminResponse(this User user)
        {
            return new UserUpdateByAdminResponseDto
            {
                UserID = user.UserId,
                Name = user.Name,
                RoleID = user.RoleId,
                Phone = user.Phone,
                Email=user.Email,
                Status = user.Status
            };
        }
    }
}