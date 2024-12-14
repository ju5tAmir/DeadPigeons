using System.Security.Claims;
using DataAccess.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.Auth.Dto;
using Service.Security;

namespace Service.Auth;

public interface IAuthService
{
    Task<RegisterResponse> Register(
        IOptions<AppOptions> options,
        UserManager<User> userManager,
        IEmailSender<User> emailSender,
        IValidator<RegisterRequest> validator,
        RegisterRequest data
    );
    
    Task<LoginResponse>  Login(
        SignInManager<User> signInManager,
        IValidator<LoginRequest> validator,
        ITokenClaimsService tokenClaimsService,
        LoginRequest data
        );

    Task<IResult> Logout(SignInManager<User> signInManager);

    Task<UserInfo> UserInfo(UserManager<User> userManager, ClaimsPrincipal principal);

    Task<ConfirmResponse> Confirm(
        UserManager<User> userManager,
        string token,
        string email);

    Task<IResult> Activate(UserManager<User> userManager, IValidator<ActivateRequest> validator, ActivateRequest data);
    

}