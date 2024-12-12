using FluentValidation;

namespace Service.Game.Dto;

public record StartGameRequest(
    int Year,
    int Week
    );

public class StartGameRequestValidator : AbstractValidator<StartGameRequest>
{
    public StartGameRequestValidator()
    {
        RuleFor(x => x.Year)
            .NotEmpty();
        RuleFor(x => x.Week)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(53);
    }
}


public record FinishGameRequest(
    Guid GameId,
    HashSet<int> WinningSequence
);

public class FinishGameRequestValidator : AbstractValidator<FinishGameRequest>
{
    public FinishGameRequestValidator()
    {
        RuleFor(x => x.GameId).NotEmpty()
            .WithMessage("GameId must not be empty.");

        RuleFor(x => x.WinningSequence)
            .NotNull()
            .WithMessage("WinningSequence must not be null.")
            .Must(set => set.Count == 3)
            .WithMessage("WinningSequence must contain exactly 3 unique numbers.")
            .ForEach(number =>
            {
                number.InclusiveBetween(1, 16)
                    .WithMessage("All numbers in WinningSequence must be between 1 and 16.");
            });
    }
}

public record UpdateOfflineProperties
(
    int Players,
    int WinningPlayers,
    int Boards,
    int WinningBoards,
    decimal Income,
    decimal Payouts
);

public class UpdateOfflinePropertiesValidator : AbstractValidator<UpdateOfflineProperties>
{
    public UpdateOfflinePropertiesValidator()
    {
        RuleFor(x => x.Players)
            .GreaterThanOrEqualTo(0).WithMessage("Players must be a positive integer.");

        RuleFor(x => x.WinningPlayers)
            .GreaterThanOrEqualTo(0).WithMessage("WinningPlayers must be a positive integer.");

        RuleFor(x => x.Boards)
            .GreaterThanOrEqualTo(0).WithMessage("Boards must be a positive integer.");

        RuleFor(x => x.WinningBoards)
            .GreaterThanOrEqualTo(0).WithMessage("WinningBoards must be a positive integer.");

        RuleFor(x => x.Income)
            .GreaterThanOrEqualTo(0).WithMessage("Income must be a positive integer.");

        RuleFor(x => x.Payouts)
            .GreaterThanOrEqualTo(0).WithMessage("Payouts must be a positive integer.");
    }
}