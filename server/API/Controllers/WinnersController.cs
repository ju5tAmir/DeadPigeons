using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Package;
using Service.Package.Dto;
using Service.Winner;
using Service.Winner.Dto;

namespace API.Controllers;

[ApiController]
[Route("/api/winner/")]
public class WinnersController(IWinnerService service)
    : ControllerBase
{
    
    [HttpGet]
    [Route("{id}")]
    public async Task<WinnersResponse> GetWinnersByGameId(Guid id)
    {
        return await service.GetGameWinnersByGameId(id);
    }
    
    
    [HttpPost]
    [Route("")]
    public async Task<WinnersResponse> GetGameWinners([FromBody] WinnersRequest data)
    {
        return await service.GetGameWinnersBySequence(data);
    }
    
}