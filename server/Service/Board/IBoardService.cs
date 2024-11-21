using System.Security.Claims;
using Service.Board.Dto;

namespace Service.Board;

public interface IBoardService
{
    Task<BoardResponse> Get(ClaimsPrincipal principal, Guid boardId);
    Task<List<BoardResponse>> GetAll(ClaimsPrincipal principal);
    Task<PlayBoardResponse> Play(ClaimsPrincipal principal, PlayBoardRequest data);
    Task<bool> Delete(ClaimsPrincipal principal, Guid boardId);
}