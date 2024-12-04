using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Service.Game;
using Service.Package.Utils;
using Service.Repositories;
using Service.Winner.Dto;
using Service.Winner.Utils;

namespace Service.Winner;

// Note: Still have to implement non-winner weekends prize
public class WinnerService( // Question: At this point IDK if I should go with repositories or service classes for these entities
    IRepository<DataAccess.Entities.Winner> winnersRepository,
    IRepository<DataAccess.Entities.Board> boardRepository,
    IRepository<DataAccess.Entities.User> userRepository,
    IRepository<DataAccess.Entities.Game> gameRepository
        ): IWinnerService
{
    public async Task<WinnersResponse> GetGameWinnersByGameId(Guid gameId)
    {
        // Fetch the game and validate
        var game = await gameRepository
            .Query()
            .FirstOrDefaultAsync(g => g.GameId == gameId);

        if (game == null)
        {
            throw new NotFoundError(nameof(Game), new { Id = gameId });
        }

        return await GetGameWinnersBySequence(new WinnersRequest(gameId, game.WinningSequence.ToHashSet()));
    }

    public async Task<WinnersResponse> GetGameWinnersBySequence(WinnersRequest data)
    {
        // Initialize the winners list
        var winners = new List<Winners>();

        // Fetch the game and validate
        var game = await gameRepository
            .Query()
            .FirstOrDefaultAsync(g => g.GameId == data.GameId);

        if (game == null)
        {
            throw new NotFoundError(nameof(Game), new { Id = data.GameId });
        }

        if (game.Status != GameStatus.Finished)
        {
            throw new GameIsNotFinished();
        }

        // Fetch all boards with the matching game ID and winning sequence
        var allBoards = await boardRepository
            .Query()
            .Where(b => b.GameId == data.GameId)
            .Where(b => data.WinningSequence.All(seq => b.PlaySequence.Contains(seq))) // Match sequence
            .Include(b => b.Package) // Include Package only
            .ToListAsync();

         // Total bets
        decimal totalPrize = allBoards.Sum(b => b.Package.Price) * GameProfits.UserProfit / 100;
        totalPrize = Math.Round(totalPrize, 2); // Round to 2 decimal places

        // Extract unique player IDs from the boards
        var playerIds = allBoards
            .Select(b => b.PlayerId)
            .Distinct()
            .ToList();

        // Fetch all players in a single query
        var players = await userRepository
            .Query()
            .Where(u => playerIds.Contains(u.Id))
            .ToListAsync();

        // Map players to a dictionary for quick lookup
        var playersDictionary = players.ToDictionary(p => p.Id, p => new PlayerDetails(
            Guid.Parse(p.Id),
            p.FirstName,
            p.LastName,
            p.UserName
        ));

        // Group boards by PlayerId
        var boardsGroupedByPlayer = allBoards
            .GroupBy(b => b.PlayerId) // Group by PlayerId
            .ToDictionary(g => g.Key, g => g.ToList()); // Convert to Dictionary for easy access

        // Process boards grouped by players
        foreach (var playerBoards in boardsGroupedByPlayer)
        {
            var playerId = playerBoards.Key;
            var playerBoardsList = playerBoards.Value;
            
            // Get player details from the dictionary
            if (!playersDictionary.TryGetValue(playerId, out var playerDetails))
            {
                throw new NotFoundError(nameof(User), new { Id = playerId });
            }

            // Calculate the total prize for the player (sum of all their board prizes)
            decimal totalWin = 0;
            var boardDetailsList = new List<BoardDetails>();

            // Calculate each board's prize
            foreach (var board in playerBoardsList)
            {
                // Calculate individual board prize as a percentage of the total prize
                decimal boardPrize = board.Package.Price / allBoards.Sum(b => b.Package.Price) * totalPrize;
                boardPrize = Math.Round(boardPrize, 2); // Round to 2 decimal places

                // Add the board prize to the total win
                totalWin += boardPrize;

                // Map board to BoardDetails
                boardDetailsList.Add(new BoardDetails(
                    board.BoardId,
                    PackageMapper.ToResponse(board.Package),
                    board.PlaySequence,
                    boardPrize
                ));
            }

            // Create the winner object
            var winner = new Winners(
                playerDetails,
                boardDetailsList,
                totalWin
            );
            winners.Add(winner);
        }

        

        // Return the mapped response
        return WinnersMapper.ToResponse(game, winners);

    }
}