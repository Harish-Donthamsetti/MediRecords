using MediRecords.Domain.Entities;
using MediRecords.Domain.Enums;
namespace MediRecords.Dto.UserDtos
{
    public class UserUpdateByAdminRequestDto
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int RoleID { get; set; }
        public bool Status { get; set; } = true;
    }
}