using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
       public async Task<CreateGameResponse> CreateGame()
       {
              return await service.CreateGame();
       }
}