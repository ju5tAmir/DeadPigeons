using Microsoft.EntityFrameworkCore;
using Service.Game.Dto;
using Service.Repositories;
using Service.Utils;

namespace Service.Game;

public class GameService(
    IRepository<DataAccess.Entities.Game> gameRepository
    ): IGameService
{
    public async Task<CreateGameResponse> CreateGame()
    {
        // Check if the week game is already started
        var isAlreadyStarted = await gameRepository
            .Query()
            .Where(g => g.WeekNumber == TimeUtils.GetCurrentWeekNumber())
            .FirstOrDefaultAsync();

        if (isAlreadyStarted != null)
        {
            throw new GameAlreadyStartedError();  // If the game for the current week already exists
        }

        // Create a new game with UTC timestamps
        var game = new DataAccess.Entities.Game
        {
            GameId = Guid.NewGuid(),
            WeekNumber = TimeUtils.GetCurrentWeekNumber(),
            ValidFromDate = TimeUtils.GetCurrentWeekStartDate(),
            ValidUntilDate = TimeUtils.GetCurrentWeekEndDate(),
            Status = "Active",
            WinningSequence = null,
            FinishedAt = null
        };

        // Add the game to the repository
        await gameRepository.Add(game);

        var gameResponse = new CreateGameResponse(
            game.GameId,
            game.WeekNumber,
            game.ValidFromDate,
            game.ValidUntilDate,
            game.Status,
            game.WinningSequence,
            game.FinishedAt
        );
        
        return gameResponse;
    }

    public async Task<CreateGameResponse> GetCurrentGame()
    {
        // Check if the week game is already started
        var game = await gameRepository
            .Query()
            .Where(g => g.WeekNumber == TimeUtils.GetCurrentWeekNumber())
            .FirstOrDefaultAsync();

        if (game == null)
        {
            throw new GameNotStartedError();  
        }

        var gameResponse = new CreateGameResponse(
            game.GameId,
            game.WeekNumber,
            game.ValidFromDate,
            game.ValidUntilDate,
            game.Status,
            game.WinningSequence,
            game.FinishedAt
        );
        
        return gameResponse;
    }
}