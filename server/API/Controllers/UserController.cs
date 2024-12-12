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
using Service.Users;

namespace API.Controllers;

[ApiController]
[Route("/api/user")]
public class UserController(IUserService service): ControllerBase
{
    [HttpGet]
    [Route("all")]
    [AllowAnonymous]
    public async Task<List<UserInfo>> ActivateAccount(
        [FromServices] UserManager<User> userManager
    )
    {
        return await service.GetAllUsers(userManager);
    }
    
    
}