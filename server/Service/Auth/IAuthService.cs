using DataAccess.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Service.Auth.Dto;
using Service.Security;

namespace Service.Auth;

public interface IAuthService
{
    Task<RegisterResponse> Register(
        IOptions<AppOptions> options,
        UserManager<User> userManager,
        IValidator<RegisterRequest> validator,
        RegisterRequest data
    );
    
    Task<LoginResponse>  Login(
        SignInManager<User> signInManager,
        IValidator<LoginRequest> validator,
        ITokenClaimsService tokenClaimsService,
        LoginRequest data
        );
}