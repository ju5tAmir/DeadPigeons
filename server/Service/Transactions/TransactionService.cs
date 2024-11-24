using System.Net.Http.Headers;
using System.Security.Claims;
using DataAccess.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Service.Auth;
using Service.Authorization;
using Service.Repositories;
using Service.Security;
using Service.Transactions.Dto;
using Service.Transactions.Helper;
using Service.Transactions.Utils;

namespace Service.Transactions;

public class TransactionService( // Note: Implement proper access control for admin
        IRepository<Transaction> transactionRepository,
        IRepository<MobilePayPayment> mobilePayRepository,
        IValidator<CreateTransactionRequest> validator,
        IAuthority authority,
        IRepository<User> userRepository
        )
    : ITransactionService
{
    public async Task<TransactionResponse> Create(ClaimsPrincipal principal, CreateTransactionRequest data)
    {
        await validator.ValidateAndThrowAsync(data);
        await authority.AuthorizeAndThrowAsync(principal);
        
        // Get the target user
        var user = await userRepository
            .Query()
            .Where(u => u.Id == data.UserId.ToString())
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new NotFoundError(nameof(User), new { Id = data.UserId });
        }
        // Access Control 
        if (principal.GetUserId() != user.Id)
        {
            throw new ForbiddenError();
        }
        
        // Role Control
        
        var transaction = new Transaction()
        {
            TransactionId = Guid.NewGuid(),
            UserId = user.Id,
            Type = data.TransactionType,
            PaymentMethod = data.PaymentMethod,
            Amount = data.Amount,
            Status = TransactionStatus.Pending,
            TransactionDate = DateTime.UtcNow
        };

        // Process Transaction
        await Process(transaction, user);

        return TransactionMapper.ToResponse(transaction);
    }

    public async Task<TransactionResponse> GetTransactionById(ClaimsPrincipal principal, Guid transactionId)
    {
        await authority.AuthorizeAndThrowAsync(principal);
        
        // Get the target user
        var user = await userRepository
            .Query()
            .Where(u => u.Id == principal.GetUserId())
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new ForbiddenError();
        }

        var transaction = await transactionRepository
            .Query()
            .Where(t => t.TransactionId == transactionId)
            .FirstOrDefaultAsync();

        if (transaction == null)
        {
            throw new NotFoundError(nameof(Transaction), transactionId);
        }

        return TransactionMapper.ToResponse(transaction);
    }

    public async Task<List<TransactionResponse>> GetAllTransactions(ClaimsPrincipal principal)
    {
        await authority.AuthorizeAndThrowAsync(principal);
        
        // Get the target user
        var user = await userRepository
            .Query()
            .Where(u => u.Id == principal.GetUserId())
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new ForbiddenError();
        }

        var transactions = await transactionRepository
            .Query()
            .Where(u => u.UserId == user.Id)
            .Select(t => TransactionMapper.ToResponse(t))
            .ToListAsync();

        return transactions;
    }

    private async Task ProcessDeposit(Transaction transaction, User user)
    {
        
        switch (transaction.PaymentMethod)
        {
            case PaymentMethod.MobilePay:
                await ProcessMobilePay(transaction, user);
                return;
            case PaymentMethod.MobilePayManual:
                return;
            case PaymentMethod.System:
                return;
        }
    }

    
    private async Task Process(Transaction transaction, User user)
    {
        switch (transaction.Type)
        {
            case TransactionType.Deposit:
                await ProcessDeposit(transaction, user);
                return;
            case TransactionType.Withdrawal:
                throw new NotImplementedException();
                return;
        }
    }
    
    private async Task ProcessMobilePay(Transaction transaction, User user)
    {
        // Call to MobilePay API with number and amount
        
        // Assuming successful respond from mobile pay
        var mobilePay = new MobilePayPayment()
        {
            PaymentId = Guid.NewGuid(),
            TransactionId = transaction.TransactionId,
            CustomerName = $"{user.FirstName} {user.LastName}", // it should be from mobile pay api
            PaymentPhone = user.PhoneNumber ?? "123"
        };

        transaction.Status = TransactionStatus.Complete;
        user.Balance += transaction.Amount;
        
        await transactionRepository
            .Add(transaction);

        await mobilePayRepository
            .Add(mobilePay);

        await userRepository
            .Update(user);
    }
 
}