using DataAccess.Entities;
using Service.Transactions.Dto;

namespace Service.Transactions.Helper;

public class TransactionMapper
{
    public static TransactionResponse ToResponse(Transaction transaction)
    {
        return new TransactionResponse(
            transaction.TransactionId,
            transaction.PaymentMethod,
            transaction.Type,
            transaction.Amount,
            transaction.Status,
            transaction.TransactionDate
            );
    }
}