using DataAccess.Entities;
using Service.Board.Dto;
using Service.Package.Utils;

namespace Service.Board.Utils;

public class BoardMapper
{
    public static BoardResponse ToResponse(
        DataAccess.Entities.Board board,
        DataAccess.Entities.Game game,
        DataAccess.Entities.Package package)
    {
        return new BoardResponse(
            board.BoardId,
            GameMapper.ToResponse(game),
            PackageMapper.ToResponse(package),
            board.PlaySequence
                .Split(", ")
                .Select(int.Parse)
                .ToHashSet() ,
            board.PlayDate
            
        );
    }
}