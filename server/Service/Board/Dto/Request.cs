using FluentValidation;

namespace Service.Board.Dto;

public record PlayBoardRequest(
    Guid GameId,
    Guid PackageId,
    HashSet<int> PlaySequence // Use HashSet<int> for uniqueness
);

public class PlayBoardRequestValidator : AbstractValidator<PlayBoardRequest>
{
    public PlayBoardRequestValidator()
    {
        RuleFor(x => x.GameId)
            .NotEmpty().WithMessage("GameId is required.");
        
        RuleFor(x => x.PackageId)
            .NotEmpty().WithMessage("PackageId is required.");

        RuleFor(x => x.PlaySequence)
            .NotEmpty().WithMessage("PlaySequence is required.")
            .Must(ContainValidNumbers).WithMessage("Each number must be between 1 and 16.");
    }

    private bool ContainValidNumbers(HashSet<int> sequence)
    {
        // Ensure all numbers are between 1 and 16
        return sequence.All(number => number >= 1 && number <= 16);
    }
}