using FluentValidation;
using Service.Transactions.Utils;

namespace Service.Transactions.Dto;

public record CreateTransactionRequest(
    Guid UserId,
    string PaymentMethod,
    string TransactionType,
    decimal Amount
    );
    
public class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
{
    public CreateTransactionRequestValidator()
    {
        // Validate UserId
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .NotEqual(Guid.Empty).WithMessage("UserId must be a valid GUID.");

        // Validate PaymentMethod
        RuleFor(x => x.PaymentMethod)
            .NotEmpty().WithMessage("PaymentMethod is required.")
            .Must(BeAValidPaymentMethod)
            .WithMessage($"PaymentMethod must be one of: {string.Join(", ", GetValidPaymentMethods())}");

        // Validate TransactionType
        RuleFor(x => x.TransactionType)
            .NotEmpty().WithMessage("TransactionType is required.")
            .Must(BeAValidTransactionType)
            .WithMessage($"TransactionType must be one of: {string.Join(", ", GetValidTransactionTypes())}");

        // Validate Amount
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");
    }

    // Helper methods to check valid PaymentMethod and TransactionType
    private static bool BeAValidPaymentMethod(string paymentMethod)
    {
        return GetValidPaymentMethods().Contains(paymentMethod);
    }

    private static bool BeAValidTransactionType(string transactionType)
    {
        return GetValidTransactionTypes().Contains(transactionType);
    }

    // Helper methods to get valid values
    private static string[] GetValidPaymentMethods()
    {
        return new[]
        {
            PaymentMethod.MobilePay,
            PaymentMethod.MobilePayManual,
            PaymentMethod.System
        };
    }

    private static string[] GetValidTransactionTypes()
    {
        return new[]
        {
            TransactionType.Withdrawal,
            TransactionType.Deposit
        };
    }
}