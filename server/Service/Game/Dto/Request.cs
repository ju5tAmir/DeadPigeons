using FluentValidation;

namespace Service.Game.Dto;

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