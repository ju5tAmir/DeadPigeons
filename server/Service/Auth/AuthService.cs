using DataAccess.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Service.Auth.Dto;
using Service.Security;

namespace Service.Auth;

public class AuthService: IAuthService
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
}