
using WebApi.Domain.Models;

namespace WebApi.Application.Services.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> UploadAsync(string bucketName, string fileName, IFormFile file);
    }
}
