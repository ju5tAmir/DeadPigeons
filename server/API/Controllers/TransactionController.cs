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
   [Route("{id}")]
   [AllowAnonymous]
   public async Task<TransactionResponse> GetTransactionById(Guid id)
   {
      return await service.GetTransactionById(HttpContext.User, id);
   } 
   
   [HttpPost]
   [Route("create")]
   [AllowAnonymous]
   public async Task<TransactionResponse> MakeTransaction([FromBody] CreateTransactionRequest data)
   {
      return await service.Create(HttpContext.User, data);
   } 
}