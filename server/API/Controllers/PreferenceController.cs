using DataAccess;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Auth.Dto;
using Service.Preference;
using Service.Preference.Dto;
using Service.Security;

namespace API.Controllers;

[ApiController]
[Route("/api/preferences")]
[AllowAnonymous]
public class PreferenceController(IPreferenceService preferenceService): ControllerBase
{
    private readonly IPreferenceService preferenceService = preferenceService;
    
    [HttpGet]
    [Route("{id}")]
    public PreferenceResponse Get(Guid id)
    {
        return preferenceService.GetById(id, HttpContext.User);
    }
}