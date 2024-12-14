using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Upload.Dto;

namespace Service.Upload;

public interface IUploadService
{
    Task<UploadResponse> Upload(IFormFile file);
}