using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Auth.Dto;
using Service.Users;
using Service.Users.Dto;

namespace API.Controllers;

[ApiController]
[Route("/api/user")]
public class UserController(IUserService service): ControllerBase
{
    [HttpGet]
    [Route("{id}")]
    [Authorize(Roles = Role.Admin)]
    public async Task<UserInfo> GetUserById(
        [FromServices] UserManager<User> userManager,
        Guid id
    )
    {
        return await service.GetUserById(userManager, id);
    }
    
    [HttpPost]
    [Route("")]
    [Authorize(Roles = Role.Admin)]
    public async Task<UserInfo> GetUserV2(
        [FromServices] UserManager<User> userManager,
        [FromBody] FetchUserRequest data
    )
    {
        return await service.GetUserV2(userManager, data);
    }
    
    [HttpGet]
    [Route("all")]
    [Authorize(Roles = Role.Admin)]
    public async Task<List<UserInfo>> GetAllUsers(
        [FromServices] UserManager<User> userManager
    )
    {
        return await service.GetAllUsers(userManager);
    }
    
    [HttpPut]
    [Route("{id}")]
    [Authorize(Roles = Role.Admin)]
    public async Task<bool> UpdateUser(
        [FromBody] UpdateUserRequest user, Guid id
    )
    {
        return await service.UpdateUser(id, user);
    }
    
    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = Role.Admin)]
    public async Task<bool> DeleteUser(
        Guid id
    )
    {
        return await service.DeleteUser(id);
    }
}