using Microsoft.AspNetCore.Http;

namespace Elomoas.Application.Interfaces.Services
{
    public interface IFileService
    {
        Task<string> SaveProfileImageAsync(IFormFile file, string oldImagePath = null);
        Task DeleteFileAsync(string filePath);
    }
} 