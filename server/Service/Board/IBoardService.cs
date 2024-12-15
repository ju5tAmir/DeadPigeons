using System.Security.Claims;
using Service.Board.Dto;

namespace Service.Board;

public interface IBoardService
{
    Task<BoardResponse> GetBoardById(ClaimsPrincipal principal, Guid boardId);
    Task<List<BoardResponse>> GetBoardsByUserId(Guid userId);
    Task<List<BoardResponse>> GetAllBoardsForPlayer(ClaimsPrincipal principal);
    Task<List<BoardResponse>> GetBoardsByGameId(ClaimsPrincipal principal, Guid gameId);
    Task<BoardResponse> Play(ClaimsPrincipal principal, BoardRequest data);
    Task<bool> Delete(ClaimsPrincipal principal, Guid boardId);
    Task<BoardResponse> Update(ClaimsPrincipal principal,Guid boardId, BoardRequest data);
}