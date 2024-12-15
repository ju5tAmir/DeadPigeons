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
using Service.Users.Dto;

namespace API.Controllers;

[ApiController]
[Route("/api/user")]
public class UserController(IUserService service): ControllerBase
{
    [HttpGet]
    [Route("{id}")]
    [AllowAnonymous]
    public async Task<UserInfo> GetUserById(
        [FromServices] UserManager<User> userManager,
        Guid id
    )
    {
        return await service.GetUserById(userManager, id);
    }
    
    [HttpPost]
    [Route("")]
    [AllowAnonymous]
    public async Task<UserInfo> GetUserV2(
        [FromServices] UserManager<User> userManager,
        [FromBody] string data
    )
    {
        return await service.GetUserV2(userManager, data);
    }
    
    [HttpGet]
    [Route("all")]
    [AllowAnonymous]
    public async Task<List<UserInfo>> GetAllUsers(
        [FromServices] UserManager<User> userManager
    )
    {
        return await service.GetAllUsers(userManager);
    }
    
    [HttpPut]
    [Route("{id}")]
    [AllowAnonymous]
    public async Task<bool> UpdateUser(
        [FromBody] UpdateUserRequest user, Guid id
    )
    {
        return await service.UpdateUser(id, user);
    }
    
    [HttpDelete]
    [Route("{id}")]
    [AllowAnonymous]
    public async Task<bool> DeleteUser(
        Guid id
    )
    {
        return await service.DeleteUser(id);
    }

    
}