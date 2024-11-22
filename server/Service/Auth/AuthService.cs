using System.Security.Claims;
using DataAccess.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Service.Auth.Dto;
using Service.Auth.Utils;
using Service.Preference.Utils;
using Service.Repositories;
using Service.Security;

namespace Service.Auth;

public class AuthService(
    IRepository<DataAccess.Entities.Preference> preferenceRepository
    ): IAuthService
{
    public async Task<RegisterResponse> Register(IOptions<AppOptions> options, UserManager<User> userManager, IValidator<RegisterRequest> validator, RegisterRequest data)
    {
        await validator.ValidateAndThrowAsync(data);

        var user = new User {FirstName = data.FirsName, LastName = data.LastName, UserName = data.Username, PhoneNumber = data.PhoneNumber, Email = data.Email};
        var result = await userManager.CreateAsync(user, data.Password);
        if (!result.Succeeded)
        {
            throw new ValidationError(
                result.Errors.ToDictionary(x => x.Code, x => new[] { x.Description })
            );
        }
        await userManager.AddToRoleAsync(user, Role.Player);

        var preferences = PreferenceMapper.GetDefaultPreferences(user);

        await preferenceRepository
            .Add(preferences);
        
        return new RegisterResponse(UserId:user.Id);
    }

    public async Task<LoginResponse> Login(
        SignInManager<User> signInManager,
        IValidator<LoginRequest> validator,
        ITokenClaimsService tokenClaimsService,
        LoginRequest data)
    {
        await validator.ValidateAndThrowAsync(data);
        
        var user = await signInManager.UserManager.FindByEmailAsync(data.Email) ?? throw new AuthenticationError();
        if (user.UserName == null)
        {
            throw new AuthenticationError();
        }
        
        var result = await signInManager.PasswordSignInAsync(
            user.UserName,
            data.Password,
            false,
            true
        );
        if (!result.Succeeded)
            throw new AuthenticationError();
        
        var token = await tokenClaimsService.GetTokenAsync(user.UserName);

        
        return new LoginResponse(Jwt: token);
    }

    public async Task<IResult> Logout(SignInManager<User> signInManager)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }

    public async Task<UserInfoResponse> UserInfo(UserManager<User> userManager, ClaimsPrincipal principal)
    {
        var username = principal.Identity?.Name;
        if (string.IsNullOrEmpty(username))
        {
            throw new AuthenticationError();
        }
        
        var user = await userManager.FindByNameAsync(username);
        if (user == null)
        {
            throw new AuthenticationError();
        }

        var role = (await userManager.GetRolesAsync(user)).First();

        var preference = (await preferenceRepository
                .Query()
                .Where(p => p.UserId == user.Id)
                .FirstOrDefaultAsync()
            );

        if (preference == null)
        {
            throw new NotFoundError(nameof(Preference), new { Id = "" });
        }

        return UserInfoMapper.ToResponse(user, role, preference);
    }
}