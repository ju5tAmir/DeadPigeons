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
using Service.Repositories;
using Service.Security;

namespace Service.Auth;

public class AuthService : IAuthService{
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
        
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        
        var qs = new Dictionary<string, string?> { { "token", token }, { "email", user.Email } };
        
        var confirmationLink = new UriBuilder(options.Value.Address)
        {
            Path = "/email/confirm",
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

        return UserInfoMapper.ToResponse(user, roles);
    }

    public async Task<IResult> Confirm(UserManager<User> userManager, ConfirmRequest data)
    {
        var user = await userManager.FindByEmailAsync(data.Email);
        if (user == null)
        {
            return Results.BadRequest(new {Message = "Failed to confirm your email."});
        }
        var result = await userManager.ConfirmEmailAsync(user, data.Token);
        if (!result.Succeeded)
            return Results.BadRequest(new {Message = "Failed to confirm your email."});
        
        var confirmation = await userManager.ConfirmEmailAsync(user, data.Token);
        if (!confirmation.Succeeded) 
            return Results.BadRequest(new {Message = "Failed to confirm your email."});
        
        return Results.Ok(new {Message = "Success"});
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
        
        return Results.Ok(new {Message = "Success"});
    }

    public async Task<IResult> ResetPassword(IOptions<AppOptions> options, UserManager<User> userManager, IEmailSender<User> emailSender, IValidator<ResetPasswordRequest> validator,
        ResetPasswordRequest data)
    {
        await validator.ValidateAndThrowAsync(data);

        var user = await userManager.FindByEmailAsync(data.Email);

        if (user == null)
        {
            // Brute force users prevention
            return Results.Ok(new {Message = "If your email exists in our database, you'll receive password reset link in your inbox."});
        }
        
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        
        var qs = new Dictionary<string, string?> { { "token", token }, { "email", user.Email } };
        
        var confirmationLink = new UriBuilder(options.Value.Address)
        {
            Path = "/password/change",
            Query = QueryString.Create(qs).Value
        }.Uri.ToString();

        await emailSender.SendPasswordResetLinkAsync(user, user.Email, confirmationLink);
        
        return Results.Ok(new {Message = "If your email exists in our database, you'll receive password reset link in your inbox."});
    }
}