using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Upload;
using Service.Upload.Dto;

namespace API.Controllers;

[ApiController]
[Route("/api/upload")]
[AllowAnonymous]
public class UploadController : ControllerBase
{
    private readonly IUploadService _service;

    public UploadController(IUploadService service)
    {
        _service = service;
    }

    [HttpPost("")]
    [Consumes("multipart/form-data")]
    public async Task<UploadResponse> UploadFile(IFormFile file)
    {
        return await _service.Upload(file);
    }
}
