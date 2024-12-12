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
        [FromServices] IEmailSender<User> emailSender,
        [FromServices] IValidator<RegisterRequest> validator,
        [FromBody] RegisterRequest data
    )
    {
        return await service.Register(
            options,
            userManager,
            emailSender,
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
        return await service.Logout(signInManager);
    }

    [HttpGet]
    [Route("userinfo")]
    public async Task<UserInfoResponse> UserInfo([FromServices] UserManager<User> userManager)
    {
        return await service.UserInfo(userManager, HttpContext.User);
    }
    
    [HttpGet]
    [Route("confirm")]
    [AllowAnonymous]
    public async Task<ConfirmResponse> ConfirmEmail(
        [FromServices] UserManager<User> userManager,
        string token,
        string email
    )
    {
        return await service.Confirm(userManager, token, email);
    }
    
    [HttpPost]
    [Route("activate")]
    [AllowAnonymous]
    public async Task<IResult> ActivateAccount(
        [FromServices] UserManager<User> userManager,
        [FromServices] IValidator<ActivateRequest> validator,
        [FromBody] ActivateRequest data
    )
    {
        return await service.Activate(userManager, validator, data);
    }
    
    [HttpPost]
    [Route("users")]
    [AllowAnonymous]
    public async Task<List<UserInfo>> ActivateAccount(
        [FromServices] UserManager<User> userManager
    )
    {
        return await service.GetAllUsers(userManager);
    }
    
    
}