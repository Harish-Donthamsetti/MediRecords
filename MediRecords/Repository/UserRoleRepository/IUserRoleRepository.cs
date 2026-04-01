using System;

namespace MediRecords.Repository.UserRoleRepository;

public interface IUserRoleRepository
{
    Task<bool> RoleExistsAsync(int roleId);
}
