using DataAccess.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service;
using Service.Auth;
using Service.Auth.Dto;
using Service.Security;

namespace API.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthController(IAuthService service): ControllerBase
{

    [HttpPost]
    [Route("register")]
    [AllowAnonymous]
    public async Task<RegisterResponse> Register(
        IOptions<AppOptions> options,
        [FromServices] UserManager<User> userManager,
        [FromServices] IValidator<RegisterRequest> validator,
        [FromBody] RegisterRequest data
    )
    {
        return await service.Register(
            options,
            userManager,
            validator,
            data);
    }
    
    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<LoginResponse> Login(
        [FromServices] SignInManager<User> signInManager,
        [FromServices] IValidator<LoginRequest> validator,
        [FromServices] ITokenClaimsService tokenClaimsService,
        [FromBody] LoginRequest data
    )
    {
        return await service.Login(
            signInManager,
            validator,
            tokenClaimsService,
            data);
    }

    [HttpGet]
    [Route("logout")]
    public async Task<IResult> Logout([FromServices] SignInManager<User> signInManager)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }

    [HttpGet]
    [Route("userinfo")]
    public async Task<AuthUserInfo> UserInfo([FromServices] UserManager<User> userManager)
    {
        var username = HttpContext.User.Identity?.Name;
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

        return new AuthUserInfo(
            UserId: user.Id,
            FirstName: user.FirstName,
            LastName: user.LastName,
            Username: user.UserName,
            Email: user.Email,
            PhoneNumber: user.PhoneNumber,
            Role: role,
            IsActive: user.IsActive,
            IsAutoplay: user.IsAutoPlay,
            RegisterationDate: user.RegistrationDate 
            );
    }
}