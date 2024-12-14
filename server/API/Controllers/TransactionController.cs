using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Transactions;
using Service.Transactions.Dto;

namespace API.Controllers;

[ApiController]
[Route("/api/transaction/")]
[AllowAnonymous]
public class TransactionController(ITransactionService service): ControllerBase
{
   
   [HttpGet]
   [Route("")]
   [AllowAnonymous]
   public async Task<List<TransactionResponse>> GetMyTransactions()
   {
      return await service.GetMyTransactions(HttpContext.User);
   } 
   
   [HttpGet]
   [Route("{id}")]
   [AllowAnonymous]
   public async Task<TransactionResponse> GetTransactions(Guid id)
   {
      return await service.GetTransactionById(id);
   } 
   
   [HttpGet]
   [Route("user/{id}")]
   [AllowAnonymous]
   public async Task<List<TransactionResponse>> GetUserTransactions(Guid id)
   {
      return await service.GetTransactionForUser(id);
   } 

   [HttpGet]
   [Route("all")]
   [AllowAnonymous]
   public async Task<List<TransactionResponse>> GetTransactions()
   {
      return await service.GetTransactions();
   } 
   
   
   [HttpPost]
   [Route("create")]
   [AllowAnonymous]
   public async Task<TransactionResponse> MakeTransaction([FromForm] CreateTransactionRequest data)
   {
      return await service.Create(HttpContext.User, data);
   } 

   [HttpPost]
   [Route("system")]
   [AllowAnonymous]
   public async Task<TransactionResponse> MakeTransaction([FromBody] SystemTransactionRequest data)
   {
      return await service.SystemTransactionsProcess(data);
   } 
   
   [HttpPost]
   [Route("approve/{id}")]
   [AllowAnonymous]
   public async Task<TransactionResponse> ApproveTransaction(Guid id, [FromBody] ApproveTransactionRequest data )
   {
      return await service.ApproveTransactionById(id, data);
   } 

   [HttpGet]
   [Route("decline/{id}")]
   [AllowAnonymous]
   public async Task<TransactionResponse> DeclineTransaction(Guid id)
   {
      return await service.DeclineTransactionById(id);
   } 


}