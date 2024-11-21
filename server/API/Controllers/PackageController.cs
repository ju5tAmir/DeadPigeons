using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Package;
using Service.Package.Dto;

namespace API.Controllers;


[ApiController]
[Route("/api/package/")]
[AllowAnonymous]
public class PackageController(IPackageService service)
    : ControllerBase
{
    [HttpGet]
    [Route("all")]
    public async Task<List<PackageResponse>> GetAllPackages()
    {
        return await service.GetAllPackages();
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<PackageResponse> Get(Guid id)
    {
        return await service.GetPackageById(id);
    }
   
}