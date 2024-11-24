using System.Security.Claims;
using Service.Transactions.Dto;

namespace Service.Transactions;

public interface ITransactionService
{
    Task<TransactionResponse> Create(ClaimsPrincipal principal, CreateTransactionRequest data);
    Task<TransactionResponse> GetTransactionById(ClaimsPrincipal principal, Guid transactionId);
}