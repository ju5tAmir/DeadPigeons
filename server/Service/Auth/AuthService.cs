using System.Security.Claims;
using DataAccess.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.Auth.Dto;
using Service.Auth.Utils;
using Service.Preference.Utils;
using Service.Repositories;
using Service.Security;

namespace Service.Auth;

public class AuthService(
    IRepository<DataAccess.Entities.Preference> preferenceRepository): IAuthService
{
    public async Task<RegisterResponse> Register(IOptions<AppOptions> options, UserManager<User> userManager, IEmailSender<User> emailSender , IValidator<RegisterRequest> validator, RegisterRequest data)
    {
        await validator.ValidateAndThrowAsync(data);

        // Fixing Security Issue: Duplicate email registration. #2 https://github.com/ju5tAmir/DeadPigeons/issues/2
        // var normalizedEmail = userManager.NormalizeEmail(data.Email);
        // if (await userManager.FindByEmailAsync(normalizedEmail) != null)
        // {
        //     throw new DuplicateEmail();
        // }
        
        var user = new User {FirstName = data.FirstName, LastName = data.LastName, UserName = data.Email, PhoneNumber = data.PhoneNumber, Email = data.Email};
        var result = await userManager.CreateAsync(user);
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
        
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        
        var qs = new Dictionary<string, string?> { { "token", token }, { "email", user.Email } };
        
        var confirmationLink = new UriBuilder(options.Value.Address)
        {
            Path = "/api/auth/confirm",
            Query = QueryString.Create(qs).Value
        }.Uri.ToString();

        await emailSender.SendConfirmationLinkAsync(user, user.Email, confirmationLink);
        
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

    public async Task<UserInfo> UserInfo(UserManager<User> userManager, ClaimsPrincipal principal)
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

        var roles = string.Join(", ", (await userManager.GetRolesAsync(user)));
        var preference = (await preferenceRepository
                .Query()
                .Where(p => p.UserId == user.Id)
                .FirstOrDefaultAsync()
            );

        if (preference == null)
        {
            throw new NotFoundError(nameof(Preference), new { Id = "" });
        }

        return UserInfoMapper.ToResponse(user, roles);
    }

    public async Task<ConfirmResponse> Confirm(UserManager<User> userManager, string token, string email)
    {
        var user = await userManager.FindByEmailAsync(email) ?? throw new AuthenticationError();
        var result = await userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
            throw new AuthenticationError();
        
        // Return password reset token to set the new password
        var passwordToken = await userManager.GeneratePasswordResetTokenAsync(user);
        
        return new ConfirmResponse(passwordToken);
    }

    public async Task<IResult> Activate(UserManager<User> userManager, IValidator<ActivateRequest> validator, ActivateRequest data)
    {
        await validator.ValidateAndThrowAsync(data);
        var user = await userManager.FindByEmailAsync(data.Email) ?? throw new AuthenticationError();
        var result = await userManager.ResetPasswordAsync(user, data.Token, data.Password);
        if (!result.Succeeded)
        {
            throw new AuthenticationError(); // Not using InvalidToken exception due to user enumeration
        }
        
        return Results.Ok(new {Message = "Password reset successfull."});
    }


}