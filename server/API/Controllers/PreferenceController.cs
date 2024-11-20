using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Preference;
using Service.Preference.Dto;

namespace API.Controllers;

[ApiController]
[Route("/api/preferences")]
[AllowAnonymous]
public class PreferenceController(IPreferenceService service): ControllerBase
{
    [HttpGet]
    [Route("{userId}")]
    public PreferenceResponse Get(Guid userId)
    {
        return service.GetById(userId, HttpContext.User);
    }
    
    [HttpPut]
    [Route("{userId}")]
    public async Task<PreferenceResponse> Get(Guid userId, [FromBody] PreferenceUpdateRequest data)
    {
        return await service.Update(userId, HttpContext.User , data);
    }

}