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
    [Authorize]
    public async Task<BoardResponse> GetBoardById(Guid id)
    {
        return await service.GetBoardById(HttpContext.User, id);
    }
    
    [HttpGet]
    [Route("all")]
    [Authorize]
    public async Task<List<BoardResponse>> GetAllBoardsForPlayer()
    {
        return await service.GetAllBoardsForPlayer(HttpContext.User);
    }

    [HttpGet]
    [Route("game/{id}")]
    [Authorize]
    public async Task<List<BoardResponse>> GetBoardsByGameId(Guid id)
    {
        return await service.GetBoardsByGameId(HttpContext.User, id);
    }
    
    [HttpGet]
    [Route("user/{id}")]
    [Authorize(Roles = Role.Admin)]
    public async Task<List<BoardResponse>> GetBoardsByUserId(Guid id)
    {
        return await service.GetBoardsByUserId(id);
    }
    
    [HttpPost]
    [Route("play")]
    [Authorize]
    public async Task<BoardResponse> PlayBoard([FromBody] BoardRequest data)
    {
        return await service.Play(HttpContext.User, data);
    }
    
    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = Role.Admin)]
    public async Task<bool> DeleteBoard(Guid id)
    {
        return await service.Delete(HttpContext.User, id);
    }
    
    [HttpPut]
    [Route("{id}")]
    [Authorize(Roles = Role.Admin)]
    public async Task<BoardResponse> UpdateBoard(Guid id, [FromBody] BoardRequest data)
    {
        return await service.Update(HttpContext.User, id, data);
    }
}