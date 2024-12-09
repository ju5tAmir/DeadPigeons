using DataAccess.Entities;
using Microsoft.VisualBasic.CompilerServices;
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


    public static GameLwResponse ToLightWeightResponse(Game game)
    {
        return new GameLwResponse(
            ToGameInfo(game),
            ToTimeFrame(game),
            ToLwPlayers(game),
            ToLwWinningPlayers(game),
            ToLwBoards(game),
            ToLwWinningBoards(game),
            ToLwIncome(game),
            ToLwPayouts(game)
        );
    }

    public static GameInfo ToGameInfo(Game game)
    {
        return new GameInfo(
            game.GameId,
            game.Status,
            game.WinningSequence
        );
    }
    
    public static TimeFrame ToTimeFrame(Game game)
    {
        return new TimeFrame(
            game.Year,
            game.WeekNumber,
            game.ValidFromDate,
            game.ValidUntilDate,
            game.RegisterCloseDate,
            game.FinishedAt);
    }

    private static PlayersLw ToLwPlayers(Game game)
    {
        return new PlayersLw(
            game.OnlinePlayers,
            game.OfflinePlayers,
            game.OnlinePlayers + game.OfflinePlayers);
    }
    
    private static WinningPlayersLw ToLwWinningPlayers(Game game)
    {
        return new WinningPlayersLw(
            game.OnlineWinningPlayers,
            game.OfflineWinningPlayers,
            game.OnlineWinningPlayers + game.OfflineWinningPlayers);
    }
    
    private static BoardsLw ToLwBoards(Game game)
    {
        return new BoardsLw(
            game.OnlineBoards,
            game.OfflineBoards,
            game.OnlineBoards + game.OfflineBoards);
    }
    
    private static WinningBoardsLw ToLwWinningBoards(Game game)
    {
        return new WinningBoardsLw(
            game.OnlineWinningBoards,
            game.OfflineWinningPlayers,
            game.OnlineWinningBoards + game.OfflineWinningPlayers
            );
    }
    
    private static IncomeLw ToLwIncome(Game game)
    {
        return new IncomeLw(
            game.OnlineIncome,
            game.OfflineIncome,
            game.OnlineIncome + game.OfflineIncome);
    }
    
    private static PayoutsLw ToLwPayouts(Game game)
    {
        return new PayoutsLw(
            game.OnlinePayout,
            game.OfflinePayout,
            game.OnlinePayout + game.OfflinePayout
            );
    }
    // public static GameFullResponse ToFullResponse(Game game, List<Player> players, decimal totalMoneyIn, decimal nextGameShare)
    // {
    //     return new GameFullResponse(
    //         game.GameId,
    //         game.Year,
    //         game.WeekNumber,
    //         TimeUtils.ToCet(game.ValidFromDate),
    //         TimeUtils.ToCet(game.ValidUntilDate),
    //         TimeUtils.ToCet(game.RegisterCloseDate),
    //         game.Status,
    //         game.WinningSequence?.ToArray() ?? null,
    //         game.FinishedAt.HasValue ? TimeUtils.ToCet(game.FinishedAt.Value) : null,
    //         players,
    //         totalMoneyIn,
    //         totalMoneyIn * GameProfits.OrganizerProfit / 100,
    //         totalMoneyIn * GameProfits.OrganizerProfit / 100,
    //         nextGameShare 
    //     );
    // }
}