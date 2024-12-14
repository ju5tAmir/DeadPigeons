using System.Security.Claims;
using Service.Board.Dto;

namespace Service.Board;

public interface IBoardService
{
    Task<BoardResponse> Get(ClaimsPrincipal principal, Guid boardId);
    Task<List<BoardResponse>> GetBoardsForUser(Guid userId);
    Task<List<BoardResponse>> GetAll(ClaimsPrincipal principal);
    Task<List<BoardResponse>> GetAllForGame(ClaimsPrincipal principal, Guid gameId);
    Task<BoardResponse> Play(ClaimsPrincipal principal, BoardRequest data);
    Task<bool> Delete(ClaimsPrincipal principal, Guid boardId);
    Task<BoardResponse> Update(ClaimsPrincipal principal,Guid boardId, BoardRequest data);
}