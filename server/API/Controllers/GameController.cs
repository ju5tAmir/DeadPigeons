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
       [Route("{id}")]
       [AllowAnonymous]
       public async Task<GameResponse> GetGameById(Guid id)
       {
              return await service.GetGameById(id);
       }


       [HttpGet]
       [Route("year/{year}")]
       [AllowAnonymous]
       public async Task<List<GameResponse>> GetAllGamesByYear(int year)
       {
              return await service.GetAllGamesByYear(year);
       }
       
       [HttpPost]
       [Route("start")]
       // [Authorize(Roles = Role.Admin)]
       [AllowAnonymous]
       public async Task<GameResponse> CreateGame([FromBody] StartGameRequest data)
       {
              return await service.StartGame(data);
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