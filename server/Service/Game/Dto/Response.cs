using DataAccess.Entities;
using Service.Package.Dto;

namespace Service.Game.Dto;

public record GameResponse(
    Guid GameId,
    int Year,
    int WeekNumber,
    DateTime ValidFromDate,
    DateTime ValidUntilDate,
    DateTime RegisterCloseDate,
    string? Status,
    int[]? WinningSequence,
    DateTime? FinishedAt
);

public record GameFullResponse(
    Guid GameId,
    int Year,
    int WeekNumber,
    DateTime ValidFromDate,
    DateTime ValidUntilDate,
    DateTime RegisterCloseDate,
    string? Status,
    int[]? WinningSequence,
    DateTime? FinishedAt,
    List<Player> Players,
    decimal TotalMoneyIn,
    decimal ClubShare,
    decimal PlayersShare,
    decimal NextGameShare
    
);

public record Player(
    PlayerDetails PlayerDetails,
    List<BoardDetails> PlayedBoards,
    decimal Prize
);
    
public record PlayerDetails(
    Guid PlayerId,
    string FirstName,
    string LastName,
    string Username
);

public record BoardDetails(
    Guid BoardId,
    PackageResponse PackageDetails,
    List<int> PlaySequence,
    decimal Prize
);

public record PackageDetails(
    Guid PackageId,
    int NumberOfFields,
    decimal Price
);