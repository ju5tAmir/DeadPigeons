using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Upload.Dto;

namespace Service.Upload;

public interface IUploadService
{
    Task<UploadResponse> Upload(IFormFile file);
}
public class MockUploadService : IUploadService 
{
    public Task<UploadResponse> Upload(IFormFile file)
    {
        return Task.FromResult(new UploadResponse("test", "test"));
    }
}