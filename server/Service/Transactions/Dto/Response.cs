namespace Service.Transactions.Dto;

public record TransactionResponse(
    Guid TransactionId,
    Guid UserId,
    string PaymentMethod,
    decimal Amount,
    string Status,
    string? ImageUrl,
    string? Note,
    DateTime TransactionDate 
);