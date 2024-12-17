using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Service.Upload.Dto;

namespace Service.Upload
{
    public class UploadService : IUploadService
    {
        private readonly ILogger<UploadResponse> _logger;
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;

        public UploadService(ILogger<UploadResponse> logger)
        {
            _logger = logger;
            _storageClient = StorageClient.Create(GoogleCredential.GetApplicationDefault());

            _bucketName = Environment.GetEnvironmentVariable("BUCKET_NAME");
            
            
            if (string.IsNullOrEmpty(_bucketName))
            {
                throw new InvalidOperationException("Bucket name not found in environment variables.");
            }
        }

        public async Task<UploadResponse> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new UploadException();

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            using (var stream = file.OpenReadStream())
            {
                await _storageClient.UploadObjectAsync(_bucketName, fileName, file.ContentType, stream);

                var publicUrl = $"https://storage.googleapis.com/{_bucketName}/{fileName}";

                var response = new UploadResponse(
                    Title: fileName,
                    Url: publicUrl
                );

                return response;
            }
        }
    }
}