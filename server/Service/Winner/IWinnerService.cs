using Service.Winner.Dto;

namespace Service.Winner;

public interface IWinnerService
{
    Task<WinnersResponse> GetGameWinnersByGameId(Guid gameId);
    Task<WinnersResponse> GetGameWinnersBySequence(WinnersRequest data);
}