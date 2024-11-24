using DataAccess.Entities;

namespace Service.Game.Dto;

public record GameResponse(
    Guid GameId,
    int WeekNumber,
    DateTime ValidFromDate,
    DateTime ValidUntilDate,
    string? Status,
    int[]? WinningSequence,
    DateTime? FinishedAt
);