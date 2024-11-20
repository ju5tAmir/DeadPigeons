using DataAccess.Entities;

namespace Service.Game.Dto;

public record CreateGameResponse(
    Guid GameId,
    int WeekNumber,
    DateTime ValidFromDate,
    DateTime ValidUntilDate,
    string? Status,
    string? WinningSequence,
    DateTime? FinishedAt
);