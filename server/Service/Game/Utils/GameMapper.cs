using DataAccess.Entities;
using Service.Game.Dto;

public static class GameMapper
{
    public static GameResponse ToResponse(Game game)
    {
        return new GameResponse(
            game.GameId,
            game.WeekNumber,
            game.ValidFromDate,
            game.ValidUntilDate,
            game.Status,
            game.WinningSequence?.ToArray() ?? null,
            game.FinishedAt
        );
    }
}