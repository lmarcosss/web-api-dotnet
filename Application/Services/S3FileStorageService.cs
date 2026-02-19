using Amazon.S3;
using Amazon.S3.Transfer;
using WebApi.Application.Services.Interfaces;

namespace WebApi.Application.Services
{
    public class S3FileStorageService : IFileStorageService
    {
        private readonly IAmazonS3 _s3Client;

        public S3FileStorageService(IAmazonS3 s3Client, IConfiguration config)
        {
            _s3Client = s3Client;
        }

        public async Task<string> UploadAsync(string bucketName, string fileName, IFormFile file)
        {
            using var newMemoryStream = new MemoryStream();
            file.CopyTo(newMemoryStream);

            var fileTransferUtility = new TransferUtility(_s3Client);

            var request = new TransferUtilityUploadRequest
            {
                BucketName = bucketName,
                Key = fileName,
                InputStream = newMemoryStream,
                ContentType = "image/png"
            };

            await fileTransferUtility.UploadAsync(request);

            return getFileUrl(bucketName, fileName);
        }

        private string getFileUrl(string bucketName, string fileName)
        {
            return $"https://{bucketName}.s3.{_s3Client.Config.RegionEndpoint.SystemName}.amazonaws.com/{fileName}";
        }
    }
}