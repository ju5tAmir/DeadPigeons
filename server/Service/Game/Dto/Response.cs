using DataAccess.Entities;

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