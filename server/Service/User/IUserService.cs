using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Service.Auth.Dto;
using Service.Users.Dto;

namespace Service.Users;

public interface IUserService
{
    Task<UserInfo> GetUserById(UserManager<User> userManager, Guid id);
    Task<UserInfo> GetUserV2(UserManager<User> userManager, string data);
    Task<List<UserInfo>> GetAllUsers(UserManager<User> userManager);

    Task<bool> UpdateUser(Guid id, UpdateUserRequest user);
    Task<bool> DeleteUser(Guid id);
}