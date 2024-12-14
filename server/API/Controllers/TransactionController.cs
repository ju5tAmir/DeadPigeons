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
   [HttpPost]
   [Route("create")]
   [AllowAnonymous]
   public async Task<TransactionResponse> MakeTransaction([FromForm] CreateTransactionRequest data)
   {
      return await service.Create(HttpContext.User, data);
   } 
   
   [HttpGet]
   [Route("all")]
   [AllowAnonymous]
   public async Task<List<TransactionResponse>> GetTransactions()
   {
      return await service.GetTransactions();
   } 

   [HttpPost]
   [Route("approve/{id}")]
   [AllowAnonymous]
   public async Task<TransactionResponse> ApproveTransaction(Guid id, [FromBody] decimal amount )
   {
      return await service.ApproveTransactionById(id, amount);
   } 



}