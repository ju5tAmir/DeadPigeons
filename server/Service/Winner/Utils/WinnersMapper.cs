using DataAccess.Entities;
using Service.Auth.Utils;
using Service.Board.Utils;
using Service.Game.Dto;
using Service.Package.Utils;
using Service.Winner.Dto;

namespace Service.Winner.Utils;

public class WinnersMapper
{
    public static WinnersResponse ToResponse(
        DataAccess.Entities.Game game,
        List<Winners> winners
        )
    {
        var gameDetails = GameMapper.ToResponse(game);

        return new WinnersResponse(gameDetails, winners);
    }
}

