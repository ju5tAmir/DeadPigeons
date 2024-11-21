using FluentValidation;

namespace Service.Game.Dto;

public record FinishGameRequest(
    Guid GameId,
    NumberSet WinningSequence
    );


public record NumberSet(int Number1, int Number2, int Number3);

public class FinishGameRequestValidator : AbstractValidator<FinishGameRequest>
{
    public FinishGameRequestValidator()
    {
        RuleFor(x => x.GameId).NotEmpty()
            .WithMessage("GameId must not be empty.");

        RuleFor(x => x.WinningSequence).SetValidator(new NumberSetValidator());
    }
}

public class NumberSetValidator : AbstractValidator<NumberSet>
{
    public NumberSetValidator()
    {
        RuleFor(x => x.Number1).InclusiveBetween(1, 16)
            .WithMessage("Number1 must be between 1 and 16.");

        RuleFor(x => x.Number2).InclusiveBetween(1, 16)
            .WithMessage("Number2 must be between 1 and 16.");

        RuleFor(x => x.Number3).InclusiveBetween(1, 16)
            .WithMessage("Number3 must be between 1 and 16.");
    }
}