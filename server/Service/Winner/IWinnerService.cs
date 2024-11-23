using Service.Winner.Dto;

namespace Service.Winner;

public interface IWinnerService
{
    Task<WinnersResponse> GetGameWinnersBySequence(WinnersRequest data);
}