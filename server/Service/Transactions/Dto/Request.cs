using FluentValidation;
using Microsoft.AspNetCore.Http;
using Service.Transactions.Enums;
using Service.Transactions.Utils;

namespace Service.Transactions.Dto;

public record CreateTransactionRequest(
    IFormFile ImageFile,
    string? Note
);

public record SystemTransactionRequest(
    Guid UserId,
    string Operation,
    decimal Amount,
    string? Note
);

public class SystemTransactionRequestValidator : AbstractValidator<SystemTransactionRequest>
{
    public SystemTransactionRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId cannot be empty.");
        
        RuleFor(x => x.Operation)
            .NotEmpty().WithMessage("Operation is required.")
            .Must(BeAValidSystemOperation)
            .WithMessage($"Operation must be either '{SystemOperation.Add}' or '{SystemOperation.Remove}'.");
        
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");

        RuleFor(x => x.Note)
            .MaximumLength(300).WithMessage("Note content cannot exceed 300 characters");
    }

    private static bool BeAValidSystemOperation(string systemOperation)
    {
        return GetValidSystemOperations().Contains(systemOperation);
    }
    
    private static string[] GetValidSystemOperations()
    {
        return new[]
        {
            SystemOperation.Add,
            SystemOperation.Remove
        };
    }
}

public class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
{
    public CreateTransactionRequestValidator()
    {
        RuleFor(x => x.ImageFile)
            .NotEmpty().WithMessage("Image file cannot be empty.");
        
        RuleFor(x => x.Note)
            .MaximumLength(300).WithMessage("Note content cannot exceed 300 characters.");
    }
}