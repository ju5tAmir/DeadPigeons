using DataAccess.Entities;
using Service.Transactions.Dto;

namespace Service.Transactions.Helper;

public class TransactionMapper
{
    public static TransactionResponse ToResponse(Transaction transaction, ManualPayment manual)
    {
        return new TransactionResponse(
            transaction.TransactionId,
            transaction.PaymentMethod,
            transaction.Amount,
            transaction.Status,
            manual.ImagePath,
            manual.Note,
            transaction.TransactionDate
            );
    }
}