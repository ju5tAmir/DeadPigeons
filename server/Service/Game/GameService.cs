using DataAccess.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Service.Game.Dto;
using Service.Package.Utils;
using Service.Repositories;
using Service.Utils;
using Service.Winner;

namespace Service.Game;

public class GameService(
    IRepository<DataAccess.Entities.Game> gameRepository,
    IRepository<DataAccess.Entities.Board> boardRepository,
    IRepository<User> userRepository,
    IWinnerService winnerService,
    IValidator<FinishGameRequest> validator,
        IValidator<UpdateOfflineProperties> updateValidator
    ): IGameService
{
    public async  Task<List<GameResponse>> GetAllGamesByYear(int year)
    {
       return await gameRepository
            .Query()
            .Where(g => g.Year == year)
            .OrderBy(g => g.WeekNumber)
            .Select(g => GameMapper.ToResponse(g))
            .ToListAsync();
    }

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

    public async Task<GameResponse> GetGameFullDetails(Guid gameId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<GameLwResponse>> GetGamesLightWeightResponse(int year)
    {
        return await gameRepository
            .Query()
            .Where(g => g.Year == year)
            .OrderBy(g => g.WeekNumber)  
            .Select(g => GameMapper.ToLightWeightResponse(g))
            .ToListAsync();
    }

    public async Task<List<int>> GetYears()
    {
        return await gameRepository
            .Query()
            .Select(g => g.Year)
            .Distinct()
            .ToListAsync();
    }

    public async Task<GameLwResponse> GetLightWeightGameById(Guid id)
    {
        var game = await gameRepository
            .Query()
            .Where(g => g.GameId == id)
            .Select(g => GameMapper.ToLightWeightResponse(g))
            .FirstOrDefaultAsync();

        if (game == null)
        {
            throw new NotFoundError(nameof(Game), new { Id = id });
        }

        return game;
    }

    public async Task<List<GamePlayerDetails>> GetPlayersForGame(Guid gameId)
    {
        return await userRepository
            .Query()
            .Join(
                boardRepository.Query(),
                user => user.Id,            
                board => board.PlayerId,      
                (user, board) => new { user, board } 
            )
            .Where(joined => joined.board.GameId == gameId)
            .Select(joined => GameMapper.ToPlayer(joined.user))
            .ToListAsync();
    }

    public async Task<List<GameBoardsDetails>> GetGameBoards(Guid id)
    {
        return await userRepository // UserRepository? because AspNetUser is not User :(  
            .Query()
            .Join(
                boardRepository.Query()
                    .Include(board => board.Package),
                user => user.Id,             
                board => board.PlayerId,    
                (user, board) => new { user, board }
            )
            .Where(joined => joined.board.GameId == id) 
            .Select(joined => GameMapper.ToBoardDetails(joined.user, joined.board)) 
            .ToListAsync(); 
    }

    public async Task<GameLwResponse> UpdateGameOfflineProperties(Guid id, UpdateOfflineProperties data)
    {
        await updateValidator.ValidateAndThrowAsync(data);

        var game = await gameRepository
            .Query()
            .Where(g => g.GameId == id)
            .FirstOrDefaultAsync();

        if (game == null)
        {
            throw new NotFoundError(nameof(Game), new { Id = id });
        }

        game.OfflinePlayers = data.Players;
        game.OfflineWinningPlayers = data.WinningPlayers;
        game.OfflineBoards = data.Boards;
        game.OfflineWinningPlayers = data.WinningBoards;
        game.OfflineIncome = data.Income;
        game.OfflinePayout = data.Payouts;

        await gameRepository
            .Update(game);

        return GameMapper.ToLightWeightResponse(game);
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