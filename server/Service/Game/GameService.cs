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
        // // Fetching the game
        // var game = await gameRepository
        //     .Query()
        //     .Where(g => g.GameId == gameId)
        //     .FirstOrDefaultAsync();
        //
        // // Fetching the boards related to the game
        // var boardsList = await boardRepository
        //     .Query()
        //     .Where(b => b.GameId == gameId)
        //     .Include(b => b.Package)
        //     .Include(b => b.Player)
        //     .ToListAsync();
        //
        // // Fetch all the players objects
        // var players = boardsList
        //     .Select(b => b.Player)
        //     .ToList()
        //     .Distinct();
        //
        // // Extract unique player IDs from the boards 
        // var playerIds = boardsList
        //     .Select(b => b.PlayerId)
        //     .Distinct()
        //     .ToList();
        //
        //
        // // Map players to a dictionary for quick lookup
        // var playersDictionary = players.ToDictionary(p => p.Id, p => new GamePlayerDetails(
        //     Guid.Parse(p.Id),
        //     p.FirstName,
        //     p.LastName,
        //     p.UserName,
        //     p.IsAutoPlay
        // ));
        //
        // // Group boards by PlayerId
        // var boardsGroupedByPlayer = boardsList
        //     .GroupBy(b => b.PlayerId) // Group by PlayerId
        //     .ToDictionary(g => g.Key, g => g.ToList()); // Convert to Dictionary for easy access
        //
        // // Total bets
        // decimal totalPrize = boardsList.Sum(b => b.Package.Price) * GameProfits.UserProfit / 100;
        // totalPrize = Math.Round(totalPrize, 2); // Round to 2 decimal places
        //
        // List<Player> playersList = new List<Player>();
        // // Process boards grouped by players
        // foreach (var playerBoards in boardsGroupedByPlayer)
        // {
        //     var playerId = playerBoards.Key;
        //     var playerBoardsList = playerBoards.Value;
        //
        //     // Get player details from the dictionary
        //     if (!playersDictionary.TryGetValue(playerId, out var playerDetails))
        //     {
        //         throw new NotFoundError(nameof(User), new { Id = playerId });
        //     }
        //
        //     // Calculate the total prize for the player (sum of all their board prizes)
        //     decimal totalWin = 0;
        //     var boardDetailsList = new List<GameBoardsDetails>();
        //
        //     // Calculate each board's prize
        //     foreach (var board in playerBoardsList)
        //     {
        //         // Calculate individual board prize as a percentage of the total prize
        //         decimal boardPrize = board.Package.Price / boardsList.Sum(b => b.Package.Price) * totalPrize;
        //         boardPrize = Math.Round(boardPrize, 2); // Round to 2 decimal places
        //
        //         // Add the board prize to the total win
        //         totalWin += boardPrize;
        //
        //         // Map board to BoardDetails
        //         boardDetailsList.Add(new GameBoardsDetails(
        //             board.BoardId,
        //             PackageMapper.ToResponse(board.Package),
        //             board.PlaySequence
        //             // boardPrize
        //         ));
        //     }
        //
        //     var player = new Player(
        //         playerDetails,
        //         boardDetailsList
        //         );
        //     
        //     playersList.Add(player);
        // }
        //
        // throw new NotImplementedException();
        // // return new GameFullResponse();
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