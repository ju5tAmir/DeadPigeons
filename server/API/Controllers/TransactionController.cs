using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Transactions;
using Service.Transactions.Dto;

namespace API.Controllers;

[ApiController]
[Route("/api/transaction/")]
public class TransactionController(ITransactionService service): ControllerBase
{
   
   [HttpGet]
   [Route("")]
   [Authorize]
   public async Task<List<TransactionResponse>> GetMyTransactions()
   {
      return await service.GetMyTransactions(HttpContext.User);
   } 
   
   [HttpGet]
   [Route("{id}")]
   [Authorize]
   public async Task<TransactionResponse> GetTransactionById(Guid id)
   {
      return await service.GetTransactionById(HttpContext.User, id);
   } 
   
   [HttpGet]
   [Route("user/{id}")]
   [Authorize(Roles = Role.Admin)]
   public async Task<List<TransactionResponse>> GetTransactionsByUserId(Guid id)
   {
      return await service.GetTransactionsByUserId(id);
   } 

   [HttpGet]
   [Route("all")]
   [Authorize(Roles = Role.Admin)]
   public async Task<List<TransactionResponse>> GetTransactions()
   {
      return await service.GetTransactions();
   } 
   
   [HttpPost]
   [Route("create")]
   [Authorize]
   public async Task<TransactionResponse> CreateManualTransactions([FromForm] CreateTransactionRequest data)
   {
      return await service.CreateManualTransactions(HttpContext.User, data);
   } 

   [HttpPost]
   [Route("system")]
   [Authorize(Roles = Role.Admin)]
   public async Task<TransactionResponse> SystemTransactionsProcess([FromBody] SystemTransactionRequest data)
   {
      return await service.SystemTransactionsProcess(data);
   } 
   
   [HttpPost]
   [Route("approve/{id}")]
   [Authorize(Roles = Role.Admin)]
   public async Task<TransactionResponse> ApproveTransaction(Guid id, [FromBody] ApproveTransactionRequest data )
   {
      return await service.ApproveTransactionById(id, data);
   } 

   [HttpGet]
   [Route("decline/{id}")]
   [Authorize(Roles = Role.Admin)]
   public async Task<TransactionResponse> DeclineTransaction(Guid id)
   {
      return await service.DeclineTransactionById(id);
   } 
}