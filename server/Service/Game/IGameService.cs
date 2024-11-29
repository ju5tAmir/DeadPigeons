using Service.Game.Dto;

namespace Service.Game;

public interface IGameService
{
    Task<GameResponse> GetCurrentGame();
    Task<List<GameResponse>> GetAllGamesByYear(int year);
    Task<GameResponse> GetGameById(Guid guid);
    Task<GameResponse> StartGame(StartGameRequest data);
    Task<GameResponse> FinishGame(FinishGameRequest data);
}