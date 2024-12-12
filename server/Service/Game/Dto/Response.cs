using System.Runtime.InteropServices.JavaScript;
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



// Game light-weight response to fill the overview table
public record GameLwResponse(
    GameInfo GameInfo,
    TimeFrame TimeFrame,
    PlayersLw Players,
    WinningPlayersLw WinningPlayers,
    BoardsLw Boards,
    WinningBoardsLw WinningBoards,
    IncomeLw Income,
    PayoutsLw Payouts
    );

public record PlayersLw(
    int OnlinePlayers,
    int OfflinePlayers,
    int TotalPlayers
    );

public record WinningPlayersLw(
    int OnlineWinningPlayers,
    int OfflineWinningPlayers,
    int TotalWinningPlayers
);

public record BoardsLw(
    int OnlineBoards,
    int OfflineBoards,
    int TotalBoards
    );

public record WinningBoardsLw(
    int OnlineWinningBoards,
    int OfflineWinningBoards,
    int TotalWinningBoards
);

public record IncomeLw(
    decimal OnlineIncome,
    decimal OfflineIncome,
    decimal TotalIncome
    );


public record PayoutsLw(
    decimal OnlinePayouts,
    decimal OfflinePayouts,
    decimal TotalPayouts
);

public record TimeFrame(
    int Year,
    int WeekNumber,
    DateTime ValidFromDate,
    DateTime ValidUntilDate,
    DateTime RegisterCloseDate,
    DateTime? FinishedAt
    );

public record GameInfo(
    Guid GameId,
    string? Status,
    List<int>? WinningSequence
);

public record GamePlayersResponse(
    List<GamePlayerDetails> Players
);

public record GameFullResponse(
    Guid GameId,
    int Year,
    int WeekNumber,
    TimeFrame TimeFrame,
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
    GamePlayerDetails GamePlayerDetails,
    List<GameBoardsDetails> PlayedBoards
);

public record PlayerWithBoards(
    GamePlayerDetails GamePlayerDetails,
    List<GameBoardsDetails> PlayedBoards
);

public record Players(
    List<Player> OnlinePlayers,
    int OfflinePlayers);


public record GamePlayerDetails(
    Guid PlayerId,
    string FirstName,
    string LastName,
    string Username,
    bool AutoPlay
);

public record GameBoardsDetails(
    Guid BoardId,
    GamePlayerDetails Player,
    PackageResponse PackageDetails,
    List<int> PlaySequence,
    DateTime PlayDate
    // decimal Prize
);

public record PackageDetails(
    Guid PackageId,
    int NumberOfFields,
    decimal Price
);