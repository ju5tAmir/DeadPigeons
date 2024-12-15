using System.Runtime.CompilerServices;
using DataAccess.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Auth.Dto;
using Service.Auth.Utils;
using Service.Repositories;
using Service.Users.Dto;

namespace Service.Users;

public class UserService(    
    IRepository<User> userRepository,
    IValidator<UpdateUserRequest> updateValidator
)
    : IUserService
{

    public async Task<UserInfo> GetUserById(UserManager<User> userManager, Guid id)
    {
        var user = await userRepository
            .Query()
            .Where(u => u.Id == id.ToString())
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new NotFoundError(nameof(User), new { Id = id });
        }
        
        var roles = await userManager.GetRolesAsync(user); 
        var userInfo = UserInfoMapper.ToResponse(user, string.Join(", ", roles));

        return userInfo;
    }

    public async Task<UserInfo> GetUserV2(UserManager<User> userManager, FetchUserRequest data)
    {
        bool isEmail = data.Recipient.Contains("@");
        bool isGuid = data.Recipient.Contains("-");

        var user = await userRepository
            .Query()
            .Where(u => 
                (isEmail && u.Email == data.Recipient) || 
                (!isEmail && !isGuid && u.PhoneNumber == data.Recipient) || 
                (isGuid && u.Id == data.Recipient))
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new NotFoundError(nameof(User), new { Id = data });
        }
        
        var roles = await userManager.GetRolesAsync(user); 
        var userInfo = UserInfoMapper.ToResponse(user, string.Join(", ", roles));

        return userInfo;
    }


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

    public async Task<bool> UpdateUser(Guid id, UpdateUserRequest user)
    {
        await updateValidator.ValidateAndThrowAsync(user);
        try
        {
            // Find the existing user in the database
            var existingUser = await userRepository
                .Query()
                .Where(u => u.Id == id.ToString())
                .FirstOrDefaultAsync();
                
            if (existingUser == null)
            {
                throw new NotFoundError(nameof(User), new { Id = id });
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.IsActive = user.IsActive;
            existingUser.IsAutoPlay = user.IsAutoplay;
        
            // Save changes to the database
            await userRepository.Update(existingUser);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> DeleteUser(Guid id)
    {
        try
        {
            var user = await userRepository
                .Query()
                .Where(u => u.Id == id.ToString())
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new NotFoundError(nameof(User), new { Id = id });

            }

            await userRepository
                .Delete(user);

            return true;

        }
        catch (Exception ex)
        {
            return false;
        }
    }
}