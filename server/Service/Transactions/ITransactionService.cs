using System.Security.Claims;
using Service.Transactions.Dto;

namespace Service.Transactions;

public interface ITransactionService
{
    Task<TransactionResponse> CreateManualTransactions(ClaimsPrincipal principal, CreateTransactionRequest data);
    Task<List<TransactionResponse>> GetTransactions();
    Task<TransactionResponse> ApproveTransactionById(Guid paymentId, ApproveTransactionRequest data);
    Task<TransactionResponse> DeclineTransactionById(Guid paymentId);
    Task<TransactionResponse> GetTransactionById(ClaimsPrincipal principal, Guid id);
    Task<List<TransactionResponse>> GetTransactionsByUserId(Guid userId);
    Task<TransactionResponse> SystemTransactionsProcess(SystemTransactionRequest data);
    Task<List<TransactionResponse>> GetMyTransactions(ClaimsPrincipal principal);
}