using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Auth.Dto;
using Service.Auth.Utils;
using Service.Repositories;

namespace Service.Users;

public class UserService(    
    IRepository<User> userRepository
)
    : IUserService
{
    public async Task<List<UserInfo>> GetAllUsers(UserManager<User> userManager)
    {
        var users = await userRepository
            .Query()
            .ToListAsync();


        var usersWithRoles = new List<UserInfo>();
        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user); 
            var userInfo = UserInfoMapper.ToResponse(user, string.Join(", ", roles));
            usersWithRoles.Add(userInfo);
        }

        return usersWithRoles;
    }
}