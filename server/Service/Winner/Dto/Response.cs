using Service.Auth.Dto;
using Service.Board.Dto;
using Service.Game.Dto;
using Service.Package.Dto;

namespace Service.Winner.Dto;

public record WinnersResponse(
    GameResponse GameDetails,
    List<Winners> Winners
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
    DateTime PlayDate,
    decimal Prize
    );

public record PackageDetails(
    Guid PackageId,
    int NumberOfFields,
    decimal Price
    );
    
public record Winners(
    PlayerDetails PlayerDetails,
    List<BoardDetails> BoardDetails,
    decimal TotalPrize 
    );