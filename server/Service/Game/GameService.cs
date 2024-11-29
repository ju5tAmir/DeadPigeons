using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Service.Game.Dto;
using Service.Repositories;
using Service.Utils;
using Service.Winner;

namespace Service.Game;

public class GameService(
    IRepository<DataAccess.Entities.Game> gameRepository,
    IWinnerService winnerService,
    IValidator<FinishGameRequest> validator
    ): IGameService
{
    public async Task<GameResponse> GetGameById(Guid gameId)
    {
        var game = await gameRepository
            .Query()
            .Where(g => g.GameId == gameId)
            .FirstOrDefaultAsync();

        if (game == null)
        {
            throw new NotFoundError(nameof(Game), new { Id = gameId });
        }

        return GameMapper.ToResponse(game);
    }

    public async Task<GameResponse> StartGame(StartGameRequest data)
    {
        int y = data.Year;
        int w = data.Week;
        int currentYear = DateTime.Now.Year;
        int currentWeek = TimeUtils.GetCurrentWeekNumber();
        
        // Check if the week game is already started
        var isAlreadyStarted = await gameRepository
            .Query()
            .Where(g => g.Year == y && g.WeekNumber == w)
            .FirstOrDefaultAsync();

        if (isAlreadyStarted != null)
        {
            throw new GameAlreadyStartedError();  // If the game for the current week already exists
        }

        if (y < currentYear ||
            y == currentYear && w < currentWeek)
        {
            throw new GameStartError();
        }
        
        var game = new DataAccess.Entities.Game
        {
            GameId = Guid.NewGuid(),
            Year = y,
            WeekNumber = w,
            ValidFromDate = TimeUtils.GetStartOfWeek(y, w),
            ValidUntilDate = TimeUtils.GetEndOfWeek(y, w),
            RegisterCloseDate = TimeUtils.RegisterCloseDate(y, w),
            Status = GameStatus.Active,
            WinningSequence = null,
            FinishedAt = null
        };

        // Add the game to the repository
        await gameRepository.Add(game);
        
        return GameMapper.ToResponse(game);
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
        game.WinningSequence = new List<int>
        {
            data.WinningSequence.ElementAt(0), 
            data.WinningSequence.ElementAt(1), 
            data.WinningSequence.ElementAt(2)  
        };

        // NOTE: Winners 
        
        // Finish the game
        game.FinishedAt = DateTime.UtcNow;
        game.Status = GameStatus.Finished;
        await gameRepository.Update(game);
        
        // Start the next week's game
        var (year, week) = TimeUtils.GetNextWeek(game.Year, game.WeekNumber);

        await StartGame(new StartGameRequest(year, week));
        
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
        
        return GameMapper.ToResponse(game);
    }
}