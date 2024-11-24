using Service.Game.Dto;

namespace Service.Game;

public interface IGameService
{
    Task<GameResponse> GetCurrentGame();
    Task<GameResponse> GetGameById(Guid guid);
    Task<GameResponse> StartGame();
    Task<GameResponse> FinishGame(FinishGameRequest data);
}