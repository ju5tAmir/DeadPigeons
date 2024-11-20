using Service.Game.Dto;

namespace Service.Game;

public interface IGameService
{
    Task<CreateGameResponse> CreateGame();
}