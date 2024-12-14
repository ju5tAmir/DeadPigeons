using FluentValidation;
using Microsoft.AspNetCore.Http;
using Service.Transactions.Utils;

namespace Service.Transactions.Dto;

public record CreateTransactionRequest(
    IFormFile ImageFile,
    string? Note
    );
    
public class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
{
    public CreateTransactionRequestValidator()
    {
        RuleFor(x => x.ImageFile)
            .NotEmpty().WithMessage("Image file cannot be empty");

        RuleFor(x => x.Note)
            .MaximumLength(300).WithMessage("Note content cannot exceed 300 characters.");
    }

    // Helper methods to check valid PaymentMethod and TransactionType
    private static bool BeAValidPaymentMethod(string paymentMethod)
    {
        return GetValidPaymentMethods().Contains(paymentMethod);
    }

    // Helper methods to get valid values
    private static string[] GetValidPaymentMethods()
    {
        return new[]
        {
            PaymentMethod.Manual,
            PaymentMethod.System
        };
    }


}