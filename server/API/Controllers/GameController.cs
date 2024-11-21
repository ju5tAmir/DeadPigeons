using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Game;
using Service.Game.Dto;

namespace API.Controllers;

[ApiController]
[Route("/api/game")]
public class GameController(IGameService service): ControllerBase
{
       
       [HttpGet]
       [Route("")]
       [AllowAnonymous]
       public async Task<GameResponse> GetCurrentGame()
       {
              return await service.GetCurrentGame();
       }
       
       [HttpGet]
       [Route("start")]
       [Authorize(Roles = Role.Admin)]
       public async Task<GameResponse> CreateGame()
       {
              return await service.StartGame();
       }
       
       [HttpPost]
       [Route("finish")]
       [AllowAnonymous] // NOTE: Change
       // [Authorize(Roles = Role.Admin)]
       public async Task<GameResponse> FinishGame([FromBody] FinishGameRequest data)
       {
              return await service.FinishGame(data);
       }
}