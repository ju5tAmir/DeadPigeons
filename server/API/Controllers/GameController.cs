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
       [Authorize]
       public async Task<GameResponse> GetCurrentGame()
       {
              return await service.GetCurrentGame();
       }
       
       [HttpGet]
       [Route("years")]
       [Authorize]
       public async Task<List<int>> GetAvailableYears()
       {
              return await service.GetYears();
       }
       
       [HttpGet]
       [Route("{id}")]
       [Authorize]
       public async Task<GameResponse> GetGameById(Guid id)
       {
              return await service.GetGameById(id);
       }

       [HttpPut]
       [Route("{id}/update-offline")]
       [Authorize(Roles = Role.Admin)]
       public async Task<GameLwResponse> UpdateGameOfflineProperties(Guid id, UpdateOfflineProperties data)
       {
              return await service.UpdateGameOfflineProperties(id, data);
       }
       
       [HttpGet]
       [Route("{id}/lw")]
       [Authorize(Roles = Role.Admin)]
       public async Task<GameLwResponse> GetLightWeightGameById(Guid id)
       {
              return await service.GetLightWeightGameById(id);
       }

       [HttpGet]
       [Route("{id}/players")]
       [Authorize(Roles = Role.Admin)]
       public async Task<List<GamePlayerDetails>> GetGamePlayersByGameId(Guid id)
       {
              return await service.GetGamePlayersByGameId(id);
       }
       
       [HttpGet]
       [Route("{id}/boards")]
       [Authorize(Roles = Role.Admin)]
       public async Task<List<GameBoardsDetails>> GetGameBoardsByGameId(Guid id)
       {
              return await service.GetGameBoardsByGameId(id);
       }
       
       [HttpGet]
       [Route("year/{year}")]
       [Authorize]
       public async Task<List<GameResponse>> GetAllGamesByYear(int year)
       {
              return await service.GetAllGamesByYear(year);
       }
       
       [HttpGet]
       [Route("year/{year}/lw")] 
       [Authorize(Roles = Role.Admin)]
       public async Task<List<GameLwResponse>> GetGamesLightWeightResponse(int year)
       {
              return await service.GetGamesLightWeightResponse(year);
       }
       
       [HttpPost]
       [Route("start")]
       [Authorize(Roles = Role.Admin)]
       public async Task<GameResponse> StartGame([FromBody] StartGameRequest data)
       {
              return await service.StartGame(data);
       }
       
       [HttpPost]
       [Route("finish")]
       [Authorize(Roles = Role.Admin)]
       public async Task<GameResponse> FinishGame([FromBody] FinishGameRequest data)
       {
              return await service.FinishGame(data);
       }
}