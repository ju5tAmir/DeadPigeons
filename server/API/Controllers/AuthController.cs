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
    [Authorize(Roles = Role.Admin)]
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
    [Authorize]
    public async Task<IResult> Logout([FromServices] SignInManager<User> signInManager)
    {
        return await service.Logout(signInManager);
    }

    [HttpGet]
    [Route("userinfo")]
    [Authorize]
    public async Task<UserInfo> UserInfo([FromServices] UserManager<User> userManager)
    {
        return await service.UserInfo(userManager, HttpContext.User);
    }
    
    [HttpPost]
    [Route("confirm")]
    [AllowAnonymous]
    public async Task<IResult> ConfirmEmail(
        [FromServices] UserManager<User> userManager,
        [FromBody] ConfirmRequest data
    )
    {
        return await service.Confirm(userManager, data);
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
    [Route("reset-password")]
    [AllowAnonymous]
    public async Task<IResult> ResetPassword(
        IOptions<AppOptions> options,
        [FromServices] UserManager<User> userManager,
        [FromServices] IEmailSender<User> emailSender,
        [FromServices] IValidator<ResetPasswordRequest> validator,
        [FromBody] ResetPasswordRequest data
    )
    {
        return await service.ResetPassword(
            options,
            userManager,
            emailSender,
            validator,
            data);
    }
}