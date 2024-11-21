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
    [HttpPost]
    [Route("play")]
    [AllowAnonymous]
    // [Authorize(Roles = Role.Player)]
    public async Task<PlayBoardResponse> PlayBoard([FromBody] PlayBoardRequest data)
    {
        return await service.Play(HttpContext.User, data);
    }
    
}