using System.Net.Http.Headers;
using System.Security.Claims;
using DataAccess.Entities;
using FluentValidation;
using Google.Apis.Util;
using Microsoft.EntityFrameworkCore;
using Service.Auth;
using Service.Authorization;
using Service.Repositories;
using Service.Security;
using Service.Transactions.Dto;
using Service.Transactions.Helper;
using Service.Transactions.Utils;
using Service.Upload;

namespace Service.Transactions;

public class TransactionService( // Note: Implement proper access control for admin
        IRepository<Transaction> transactionRepository,
        IRepository<ManualPayment> manualPaymentRepository,
        IUploadService uploadService,
        IValidator<CreateTransactionRequest> validator,
        IAuthority authority,
        IRepository<User> userRepository
        )
    : ITransactionService
{
    public async Task<TransactionResponse> Create(ClaimsPrincipal principal, CreateTransactionRequest data)
    {
        await validator.ValidateAndThrowAsync(data);
        // await authority.AuthorizeAndThrowAsync(principal);
    
        if (data.PaymentMethod == PaymentMethod.Manual)
        {
            // Complete later
        }
        return await ProcessCreateManualTransaction(principal, data);

    }

    public async Task<List<TransactionResponse>> GetTransactions()
    {
        return await transactionRepository
            .Query()
            .GroupJoin(
                manualPaymentRepository.Query(), 
                transaction => transaction.TransactionId, 
                manual => manual.TransactionId, 
                (transaction, manuals) => new { transaction, manuals }
            )
            .SelectMany(
                x => x.manuals.DefaultIfEmpty(), // Handle cases where no manual payment exists
                (x, manual) => new { x.transaction, manual }
            )
            .Select(x => TransactionMapper.ToResponse(x.transaction, x.manual))
            .ToListAsync();
    }

    public async Task<TransactionResponse> ApproveTransactionById(Guid paymentId, decimal amount)
    {
        var payment = await transactionRepository
            .Query()
            .Where(p => p.TransactionId == paymentId)
            .FirstOrDefaultAsync();

        if (payment == null)
        {
            throw new NotFoundError(nameof(Transaction), new { Id = paymentId });
        }
        
        payment.Amount = amount;
        payment.Status = TransactionStatus.Complete;

        var manualInfo = await manualPaymentRepository
            .Query()
            .Where(p => p.TransactionId == paymentId)
            .FirstOrDefaultAsync();
        
        await transactionRepository
            .Update(payment);

        return TransactionMapper.ToResponse(payment, manualInfo);
    }

    private async Task<TransactionResponse> ProcessCreateManualTransaction(ClaimsPrincipal principal, CreateTransactionRequest data)
    {
        var file = await uploadService.Upload(data.ImageFile);

        var transaction = new Transaction()
        {
            TransactionId = Guid.NewGuid(),
            PaymentMethod = PaymentMethod.Manual,
            Amount = 0,
            Status = TransactionStatus.Pending,
            UserId = principal.GetUserId(),
            TransactionDate = DateTime.UtcNow
        };

        var manual = new ManualPayment()
        {
            TransactionId = transaction.TransactionId,
            ImagePath = file.Url,
            Note = data.Note
        };
        
        await transactionRepository
            .Add(transaction);

        await manualPaymentRepository
            .Add(manual);

        return TransactionMapper.ToResponse(transaction, manual);
    }
 
}