using DataAccess.Entities;
using Service.Game;
using Service.Game.Dto;
using Service.Utils;

public static class GameMapper
{
    public static GameResponse ToResponse(Game game)
    {
        return new GameResponse(
            game.GameId,
            game.Year,
            game.WeekNumber,
            TimeUtils.ToCet(game.ValidFromDate),
            TimeUtils.ToCet(game.ValidUntilDate),
            TimeUtils.ToCet(game.RegisterCloseDate),
            game.Status,
            game.WinningSequence?.ToArray() ?? null,
            game.FinishedAt.HasValue ? TimeUtils.ToCet(game.FinishedAt.Value) : null
        );
    }
    
    public static GameFullResponse ToFullResponse(Game game, List<Player> players, decimal totalMoneyIn, decimal nextGameShare)
    {
        return new GameFullResponse(
            game.GameId,
            game.Year,
            game.WeekNumber,
            TimeUtils.ToCet(game.ValidFromDate),
            TimeUtils.ToCet(game.ValidUntilDate),
            TimeUtils.ToCet(game.RegisterCloseDate),
            game.Status,
            game.WinningSequence?.ToArray() ?? null,
            game.FinishedAt.HasValue ? TimeUtils.ToCet(game.FinishedAt.Value) : null,
            players,
            totalMoneyIn,
            totalMoneyIn * GameProfits.OrganizerProfit / 100,
            totalMoneyIn * GameProfits.OrganizerProfit / 100,
            nextGameShare 
        );
    }
}