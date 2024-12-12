using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Board;
using Service.Board.Dto;

namespace API.Controllers;

[ApiController]
[Route("/api/board/")]
public class BoardController(IBoardService service): ControllerBase
{
    [HttpGet]
    [Route("{id}")]
    [AllowAnonymous]
    // [Authorize(Roles = Role.Player)]
    public async Task<BoardResponse> GetBoard(Guid id)
    {
        return await service.Get(HttpContext.User, id);
    }
    
    [HttpGet]
    [Route("all")]
    [AllowAnonymous]
    // [Authorize(Roles = Role.Player)]
    public async Task<List<BoardResponse>> GetAllBoards()
    {
        return await service.GetAll(HttpContext.User);
    }

    [HttpGet]
    [Route("game/{id}")]
    [AllowAnonymous]
    // [Authorize(Roles = Role.Player)]
    public async Task<List<BoardResponse>> GetBoardsForGame(Guid id)
    {
        return await service.GetAllForGame(HttpContext.User, id);
    }
    
    [HttpGet]
    [Route("user/{id}")]
    [AllowAnonymous]
    // [Authorize(Roles = Role.Player)]
    public async Task<List<BoardResponse>> GetBoardForUser(Guid id)
    {
        return await service.GetBoardsForUser(id);
    }
    
    [HttpPost]
    [Route("play")]
    [AllowAnonymous]
    // [Authorize(Roles = Role.Player)]
    public async Task<BoardResponse> PlayBoard([FromBody] BoardRequest data)
    {
        return await service.Play(HttpContext.User, data);
    }
    
    [HttpDelete]
    [Route("{id}")]
    [AllowAnonymous]
    public async Task<bool> DeleteBoard(Guid id)
    {
        return await service.Delete(HttpContext.User, id);
    }
    
    [HttpPut]
    [Route("{id}")]
    [AllowAnonymous]
    public async Task<BoardResponse> UpdateBoard(Guid id, [FromBody] BoardRequest data)
    {
        return await service.Update(HttpContext.User, id, data);
    }
}