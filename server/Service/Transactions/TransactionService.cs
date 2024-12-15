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
using Service.Transactions.Enums;
using Service.Transactions.Helper;
using Service.Transactions.Utils;
using Service.Upload;

namespace Service.Transactions;

public class TransactionService( // Note: Implement proper access control for admin
        IRepository<Transaction> transactionRepository,
        IRepository<ManualPayment> manualPaymentRepository,
        IUploadService uploadService,
        IValidator<CreateTransactionRequest> createTransactionValidator,
        IValidator<ApproveTransactionRequest> approveTransactionValidator,
        IValidator<SystemTransactionRequest> systemValidator,
        IAuthority authority,
        IRepository<User> userRepository
        )
    : ITransactionService
{
    public async Task<TransactionResponse> Create(ClaimsPrincipal principal, CreateTransactionRequest data)
    {
        await createTransactionValidator.ValidateAndThrowAsync(data);
        // await authority.AuthorizeAndThrowAsync(principal);
        
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

    public async Task<TransactionResponse> ApproveTransactionById(Guid paymentId, ApproveTransactionRequest data)
    {
        await approveTransactionValidator.ValidateAndThrowAsync(data);
        
        var payment = await transactionRepository
            .Query()
            .Where(p => p.TransactionId == paymentId)
            .FirstOrDefaultAsync();

        if (payment == null)
        {
            throw new NotFoundError(nameof(Transaction), new { Id = paymentId });
        }

        if (payment.Status == TransactionStatus.Complete)
        {
            throw new PaymentAlreadyCompleted();
        }
        
        payment.Amount = data.Amount;
        payment.Status = TransactionStatus.Complete;
        
        // fetch user to add balance
        var user = await userRepository
            .Query()
            .Where(u => u.Id == payment.UserId)
            .FirstOrDefaultAsync() ?? throw new NotFoundError(nameof(User), new { Id = payment.UserId});

        user.Balance += payment.Amount;
        
        var manualInfo = await manualPaymentRepository
            .Query()
            .Where(p => p.TransactionId == paymentId)
            .FirstOrDefaultAsync();
        
        await transactionRepository
            .Update(payment);
        await userRepository
            .Update(user);

        return TransactionMapper.ToResponse(payment, manualInfo);
    }

    public async Task<TransactionResponse> DeclineTransactionById(Guid paymentId)
    {
        var payment = await transactionRepository
            .Query()
            .Where(p => p.TransactionId == paymentId)
            .FirstOrDefaultAsync();

        if (payment == null)
        {
            throw new NotFoundError(nameof(Transaction), new { Id = paymentId });
        }

        if (payment.Status == TransactionStatus.Declined)
        {
            throw new PaymentAlreadyDeclined();
        }
        
        payment.Status = TransactionStatus.Declined;

        var manualInfo = await manualPaymentRepository
            .Query()
            .Where(p => p.TransactionId == paymentId)
            .FirstOrDefaultAsync();

        await transactionRepository
            .Update(payment);
        
        return TransactionMapper.ToResponse(payment, manualInfo);

    }

    public async Task<TransactionResponse> GetTransactionById(Guid id)
    {
        var payment = await transactionRepository
            .Query()
            .Where(p => p.TransactionId == id)
            .FirstOrDefaultAsync();

        if (payment == null)
        {
            throw new NotFoundError(nameof(Transaction), new { Id = id });
        }

        var manualInfo = await manualPaymentRepository
            .Query()
            .Where(p => p.TransactionId == id)
            .FirstOrDefaultAsync();

        return TransactionMapper.ToResponse(payment, manualInfo);
    }

    public async Task<List<TransactionResponse>> GetTransactionForUser(Guid userId)
    {
        return await transactionRepository
            .Query()
            .Where(t => t.UserId == userId.ToString())
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

    public async Task<TransactionResponse> SystemTransactionsProcess(SystemTransactionRequest data)
    {
        await systemValidator.ValidateAndThrowAsync(data);
        bool isEmail = data.Recipiant.Contains("@");
        bool isGuid = data.Recipiant.Contains("-");

        var user = await userRepository
            .Query()
            .Where(u => 
                (isEmail && u.Email == data.Recipiant) || 
                (!isEmail && !isGuid && u.PhoneNumber == data.Recipiant) || 
                (isGuid && u.Id == data.Recipiant))
            .FirstOrDefaultAsync();
        
        if (user == null)
        {
            throw new NotFoundError(nameof(User), new { Recipient = data.Recipiant });
        }
        
        var transaction = new Transaction()
        {
            TransactionId = Guid.NewGuid(),
            PaymentMethod = PaymentMethod.System,
            Status = TransactionStatus.Complete,
            UserId = user.Id,
            TransactionDate = DateTime.UtcNow
        };

        var manual = new ManualPayment()
        {
            TransactionId = transaction.TransactionId,
            ImagePath = null,
            Note = data.Note
        };
        
        if (data.Operation == SystemOperation.Add)
        {
            user.Balance += data.Amount;
            transaction.Amount = data.Amount;
        }
        if (data.Operation == SystemOperation.Remove)
        {
            user.Balance -= data.Amount;
            transaction.Amount = -data.Amount;
        }

        await userRepository
            .Update(user);
        await transactionRepository
            .Add(transaction);
        await manualPaymentRepository
            .Add(manual);

        return TransactionMapper.ToResponse(transaction, manual);
    }

    public async Task<List<TransactionResponse>> GetMyTransactions(ClaimsPrincipal principal)
    {
        await authority.AuthorizeAndThrowAsync(principal);
        
        return await transactionRepository
            .Query()
            .Where(t => t.UserId == principal.GetUserId())
            .GroupJoin(
                manualPaymentRepository.Query(), 
                transaction => transaction.TransactionId, 
                manual => manual.TransactionId, 
                (transaction, manuals) => new { transaction, manuals }
            )
            .SelectMany(
                x => x.manuals.DefaultIfEmpty(),
                (x, manual) => new { x.transaction, manual }
            )
            .Select(x => TransactionMapper.ToResponse(x.transaction, x.manual))
            .ToListAsync();
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