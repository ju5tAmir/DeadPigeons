namespace Service.Winner.Dto;

public record PostGameWinnersRequest(Guid GameId);
public record WinnersRequest(Guid GameId, HashSet<int> WinningSequence);