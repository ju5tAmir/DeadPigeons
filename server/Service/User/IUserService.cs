using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Service.Auth.Dto;

namespace Service.Users;

public interface IUserService
{
    Task<List<UserInfo>> GetAllUsers(UserManager<User> userManager);

}