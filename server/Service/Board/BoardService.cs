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
    IValidator<PlayBoardRequest> validator
    ): IBoardService
{
    public async Task<BoardResponse> Get(ClaimsPrincipal principal, Guid boardId)
    {
        await authority.AuthorizeAndThrowAsync(principal);

        // Get the board
        var board = await boardRepository
            .Query()
            .Where(b => b.PlayerId == principal.GetUserId())
            .Where(b => b.BoardId == boardId)
            .FirstOrDefaultAsync();

        if (board == null)
        {
            throw new NotFoundError(nameof(Board), new { Id = boardId });
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
        
        // Get the package
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

    public async Task<PlayBoardResponse> Play(ClaimsPrincipal principal, PlayBoardRequest data)
    {
        await validator.ValidateAndThrowAsync(data);
        await authority.AuthorizeAndThrowAsync(principal);

        // Get the Game
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
        
        // Get the package
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

        
        // Check balance
        if (package.Price > user.Balance)
        {
            throw new InsufficientBalance();
        }

        user.Balance -= package.Price;
        

        var board = new DataAccess.Entities.Board()
        {
            BoardId = Guid.NewGuid(),
            GameId = game.GameId,
            PlayerId = user.Id,
            PackageId = package.PackageId,
            PlaySequence = string.Join(", ", data.PlaySequence),
            PlayDate = DateTime.UtcNow
        };

        
        await boardRepository
            .Add(board);
        await userRepository
            .Update(user);
        
        return new PlayBoardResponse(board.BoardId);
    }

    public async Task<bool> Delete(ClaimsPrincipal principal, Guid boardId)
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

        await boardRepository
            .Delete(board);

        return true;
    }
}