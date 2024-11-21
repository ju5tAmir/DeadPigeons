using Service.Game.Dto;
using Service.Package.Dto;

namespace Service.Board.Dto;

public record PlayBoardResponse(
    Guid BoardId
    );
    
public record BoardResponse(
    Guid BoardId,
    GameResponse Game,
    PackageResponse Package,
    HashSet<int> PlaySequence,
    DateTime? PlayTime
        );