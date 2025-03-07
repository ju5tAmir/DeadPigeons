using System.Security.Claims;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Service.Authorization;
using Service.Board.Dto;
using Service.Board.Utils;
using Service.Game;
using Service.Repositories;
using Service.Security;

namespace Service.Board;

public class BoardService(
    IRepository<DataAccess.Entities.Board> boardRepository,
    IRepository<DataAccess.Entities.Game> gameRepository,
    IRepository<DataAccess.Entities.User> userRepository,
    IRepository<DataAccess.Entities.Package> packageRepository,
    IAuthority authority,
    IValidator<BoardRequest> validator
    ): IBoardService
{
    public async Task<BoardResponse> GetBoardById(ClaimsPrincipal principal, Guid boardId)
    {
        await authority.AuthorizeAndThrowAsync(principal);
        
        var board = await boardRepository
            .Query()
            .Where(b => b.PlayerId == principal.GetUserId())
            .Where(b => b.BoardId == boardId)
            .FirstOrDefaultAsync();

        if (board == null)
        {
            throw new NotFoundError(nameof(Board), new { Id = boardId });
        }
        
        var game = await gameRepository
            .Query()
            .Where(g => g.GameId == board.GameId)
            .FirstOrDefaultAsync();

        if (game == null)
        {
            throw new  NotFoundError(nameof(DataAccess.Entities.Game), new { Id = board.GameId });
        }
        
        var package = await packageRepository
            .Query()
            .Where(p => p.PackageId == board.PackageId)
            .FirstOrDefaultAsync();

        if (package == null)
        {
            throw new NotFoundError(nameof(Package), new { Id = board.PackageId });
        }

        return BoardMapper.ToResponse(board, game, package);
    }

    public async Task<List<BoardResponse>> GetBoardsByUserId(Guid userId)
    {
        return await boardRepository
            .Query()
            .Where(b => b.PlayerId == userId.ToString())
            .Include(b => b.Package)
            .Include(b => b.Game)
            .Select(b => BoardMapper.ToResponse(b, b.Game, b.Package))
            .ToListAsync();
    }

    public async Task<List<BoardResponse>> GetAllBoardsForPlayer(ClaimsPrincipal principal)
    {
        return await boardRepository
            .Query()
            .Where(b => b.PlayerId == principal.GetUserId())
            .Include(b => b.Game)
            .Include(b => b.Package)
            .Select(b => BoardMapper.ToResponse(b, b.Game, b.Package))
            .ToListAsync();
    }

    public async Task<List<BoardResponse>> GetBoardsByGameId(ClaimsPrincipal principal, Guid gameId)
    {
        return await boardRepository
            .Query()
            .Where(b => b.PlayerId == principal.GetUserId())
            .Where(b => b.GameId == gameId)
            .Include(b => b.Game)
            .Include(b => b.Package)
            .Select(b => BoardMapper.ToResponse(b, b.Game, b.Package))
            .ToListAsync();
    }

    public async Task<BoardResponse> Play(ClaimsPrincipal principal, BoardRequest data)
    {
        await validator.ValidateAndThrowAsync(data);

        var game = await gameRepository
            .Query()
            .Where(g => g.GameId == data.GameId)
            .FirstOrDefaultAsync();

        if (game == null)
        {
            throw new  NotFoundError(nameof(DataAccess.Entities.Game), new { Id = data.GameId });
        }

        if (game.Status != GameStatus.Active)
        {
            throw new GameHasFinished();
        }
        
        var user = await userRepository
            .Query()
            .Where(u => u.Id == principal.GetUserId())
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new  NotFoundError(nameof(DataAccess.Entities.User), new { Id = principal.GetUserId() });
        }

        if (!user.IsActive)
        {
            throw new UserDisabled();
        }
        
        var package = await packageRepository
            .Query()
            .Where(p => p.PackageId == data.PackageId)
            .FirstOrDefaultAsync();

        if (package == null)
        {
            throw new NotFoundError(nameof(Package), new { Id = data.PackageId });
        }
        
        // If the number of choices matches the number of fields in package
        if (package.NumberOfFields != data.PlaySequence.Count)
        {
            throw new IllegalMove(package.NumberOfFields);
        }
        
        if (package.Price > user.Balance)
        {
            throw new InsufficientBalance();
        }
        
        user.Balance -= package.Price;
        game.OnlineBoards += 1;
        game.OnlinePlayers += 1;
        game.OnlineIncome += package.Price;
        
        var board = new DataAccess.Entities.Board()
        {
            BoardId = Guid.NewGuid(),
            GameId = game.GameId,
            PlayerId = user.Id,
            PackageId = package.PackageId,
            PlaySequence = data.PlaySequence.ToList(),
            PlayDate = DateTime.UtcNow
        };
        
        await boardRepository
            .Add(board);
        await userRepository
            .Update(user);
        
        return BoardMapper.ToResponse(board, game, package);
    }

    public async Task<bool> Delete(ClaimsPrincipal principal, Guid boardId)
    {
        var board = await boardRepository
            .Query()
            .Where(b => b.BoardId == boardId)
            .FirstOrDefaultAsync();

        if (board == null)
        {
            throw new NotFoundError(nameof(Board), new { Id = boardId });
        }
        
        if (board.PlayerId != principal.GetUserId())
        {
            throw new ForbiddenError();
        }
        
        // Get the Game
        var game = await gameRepository
            .Query()
            .Where(g => g.GameId == board.GameId)
            .FirstOrDefaultAsync();

        if (game == null)
        {
            throw new  NotFoundError(nameof(DataAccess.Entities.Game), new { Id = board.GameId });
        }

        if (game.Status != GameStatus.Active)
        {
            throw new GameHasFinished();
        }
        
        var user = await userRepository
            .Query()
            .Where(u => u.Id == principal.GetUserId())
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new  NotFoundError(nameof(DataAccess.Entities.User), new { Id = principal.GetUserId() });
        }

        if (!user.IsActive)
        {
            throw new UserDisabled();
        }
        
        var package = await boardRepository
            .Query()
            .Where(b => b.BoardId == boardId)
            .Include(b => b.Package)
            .Select(b => b.Package)
            .FirstOrDefaultAsync();
        
        if (package == null)
        {
            throw new NotFoundError(nameof(Package), new { Id = board.PackageId });
        }

        user.Balance += package.Price; 
        game.OnlineBoards -= 1;
        game.OnlinePlayers -= 1;
        game.OnlineIncome -= package.Price;
        
        await boardRepository
            .Delete(board);
        await userRepository
            .Update(user);
        await gameRepository
            .Update(game);
        
        return true;
    }

    public async Task<BoardResponse> Update(ClaimsPrincipal principal, Guid boardId, BoardRequest data)
    {
        await validator.ValidateAndThrowAsync(data);
        await authority.AuthorizeAndThrowAsync(principal);

        // Get the board
        var board = await boardRepository
            .Query()
            .Where(b => b.BoardId == boardId)
            .FirstOrDefaultAsync();
        
        if (board == null)
        {
            throw new NotFoundError(nameof(Board), new { Id = boardId });
        }

        if (board.PlayerId != principal.GetUserId())
        {
            throw new ForbiddenError();
        }
        
        // Get the Game
        var game = await gameRepository
            .Query()
            .Where(g => g.GameId == board.GameId)
            .FirstOrDefaultAsync();

        if (game == null)
        {
            throw new  NotFoundError(nameof(DataAccess.Entities.Game), new { Id = data.GameId });
        }

        if (game.Status != GameStatus.Active)
        {
            throw new GameHasFinished();
        }
        
        // Get the user
        var user = await userRepository
            .Query()
            .Where(u => u.Id == principal.GetUserId())
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new  NotFoundError(nameof(DataAccess.Entities.User), new { Id = principal.GetUserId() });
        }

        // If the user is disabled 
        if (!user.IsActive)
        {
            throw new UserDisabled();
        }
        
        // Get the old ass package
        var oldPackage = await boardRepository
            .Query()
            .Where(b => b.BoardId == boardId)
            .Include(b => b.Package)
            .Select(b => b.Package)
            .FirstOrDefaultAsync();
        
        if (oldPackage == null)
        {
            throw new NotFoundError(nameof(Package), new { Id = data.PackageId });
        }
        
        // Get the new package
        var newPackage = await packageRepository
            .Query()
            .Where(p => p.PackageId == data.PackageId)
            .FirstOrDefaultAsync();

        if (newPackage == null)
        {
            throw new NotFoundError(nameof(Package), new { Id = data.PackageId });
        }

        if (oldPackage.PackageId == newPackage.PackageId)
        {
            throw new UpdateError();
        }
        
        // If the number of choices matches the number of fields in package
        if (newPackage.NumberOfFields != data.PlaySequence.Count)
        {
            throw new IllegalMove(newPackage.NumberOfFields);
        }
        
        // Return the user's previous money to his account otherwise it would be HARAM
        user.Balance += oldPackage.Price;
        user.Balance -= newPackage.Price;

        // Check balance
        if (newPackage.Price > user.Balance)
        {
            throw new InsufficientBalance();
        }
        
        board.PackageId = newPackage.PackageId;
        board.PlaySequence = data.PlaySequence.ToList();

        await boardRepository
            .Update(board);
        await userRepository
            .Update(user);


        return BoardMapper.ToResponse(board, game, newPackage);
    }
}