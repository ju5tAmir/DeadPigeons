using System.Security.Claims;
using Service.Transactions.Dto;

namespace Service.Transactions;

public interface ITransactionService
{
    Task<TransactionResponse> Create(ClaimsPrincipal principal, CreateTransactionRequest data);
    Task<List<TransactionResponse>> GetTransactions();
    Task<TransactionResponse> ApproveTransactionById(Guid paymentId, decimal amount);
    Task<TransactionResponse> DeclineTransactionById(Guid paymentId);
    Task<TransactionResponse> GetTransactionById(Guid id);
    Task<List<TransactionResponse>> GetTransactionForUser(Guid userId);
    Task<TransactionResponse> SystemTransactionsProcess(SystemTransactionRequest data);
    Task<List<TransactionResponse>> GetMyTransactions(ClaimsPrincipal principal);
}