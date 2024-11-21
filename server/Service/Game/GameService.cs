using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Service.Game.Dto;
using Service.Repositories;
using Service.Utils;

namespace Service.Game;

public class GameService(
    IRepository<DataAccess.Entities.Game> gameRepository,
    IValidator<FinishGameRequest> validator
    ): IGameService
{
    public async Task<GameResponse> StartGame()
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
            Status = GameStatus.Active,
            WinningSequence = null,
            FinishedAt = null
        };

        // Add the game to the repository
        await gameRepository.Add(game);

        var gameResponse = new GameResponse(
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

    public async Task<GameResponse> FinishGame(FinishGameRequest data)
    {
        await validator.ValidateAndThrowAsync(data);

        var game = await gameRepository.Query()
            .Where(g => g.GameId == data.GameId)
            .FirstOrDefaultAsync();

        if (game == null)
        {
            throw new  NotFoundError(nameof(DataAccess.Entities.Game), new { Id = data.GameId });
        }

        if (game.Status == GameStatus.Finished)
        {
            throw new GameHasFinished();
        }
        game.WinningSequence = string.Join(", ", new[] 
        {
            data.WinningSequence.Number1,
            data.WinningSequence.Number2,
            data.WinningSequence.Number3
        });

        // NOTE: Winners 
        
        game.FinishedAt = DateTime.UtcNow;

        game.Status = GameStatus.Finished;
        
        await gameRepository.Update(game);

        return GameMapper.ToResponse(game);
    }

    public async Task<GameResponse> GetCurrentGame()
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

        var gameResponse = new GameResponse(
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