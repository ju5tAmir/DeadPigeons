namespace Service.Transactions.Dto;

public record TransactionResponse(
    Guid TransactionId,
    string PaymentMethod,
    decimal Amount,
    string Status,
    string? ImageUrl,
    string? Note,
    DateTime TransactionDate 
);