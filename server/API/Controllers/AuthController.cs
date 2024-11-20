using DataAccess.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service;
using Service.Auth.Dto;
using Service.Security;

namespace API.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthController: ControllerBase
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
        return new RegisterResponse(UserId:user.Id);
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