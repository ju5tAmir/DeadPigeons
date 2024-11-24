namespace Service.Transactions.Dto;

public record TransactionResponse(
    Guid TransactionId,
    string PaymentMethod,
    string TransactionType,
    decimal Amount,
    string Status,
    DateTime? TransactionDate  
);