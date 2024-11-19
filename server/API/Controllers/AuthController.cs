using DataAccess.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Identity.Data;
// using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service;
using Service.Auth.Dto;


namespace API.Controllers;


[ApiController]
[Route("/api/auth")]
public class AuthController: ControllerBase
{

    [HttpPost]
    [Route("register")]
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
    public async Task<LoginResponse> Login(
        [FromServices] SignInManager<User> signInManager,
        [FromServices] IValidator<LoginRequest> validator,
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
        return new LoginResponse(Username: user.UserName);
    }

}