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
       [Route("years")]
       [AllowAnonymous]
       public async Task<List<int>> GetAvailableYears()
       {
              return await service.GetYears();
       }
       
       
       [HttpGet]
       [Route("{id}")]
       [AllowAnonymous]
       public async Task<GameResponse> GetGameById(Guid id)
       {
              return await service.GetGameById(id);
       }

       [HttpGet]
       [Route("{id}/lw")]
       [AllowAnonymous]
       public async Task<GameLwResponse> GetLightWeightGameById(Guid id)
       {
              return await service.GetLightWeightGameById(id);
       }

       [HttpGet]
       [Route("{id}/players")]
       [AllowAnonymous]
       public async Task<List<GamePlayerDetails>> GetGamePlayersByGameId(Guid id)
       {
              return await service.GetPlayersForGame(id);
       }
       
       [HttpGet]
       [Route("{id}/boards")]
       [AllowAnonymous]
       public async Task<List<GameBoardsDetails>> GetGameBoards(Guid id)
       {
              return await service.GetGameBoards(id);
       }
       
       
       [HttpGet]
       [Route("year/{year}")]
       [AllowAnonymous]
       public async Task<List<GameResponse>> GetAllGamesByYear(int year)
       {
              return await service.GetAllGamesByYear(year);
       }
       
       [HttpGet]
       [Route("year/{year}/lw")] // Note: Change it to query
       [AllowAnonymous] // Note : Admin only
       public async Task<List<GameLwResponse>> GetGamesLightWeight(int year)
       {
              return await service.GetGamesLightWeightResponse(year);
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