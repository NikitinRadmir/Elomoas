using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Elomoas.Application.Interfaces.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Elomoas.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;
        private readonly IMinioService _minioService;
        private const string DEFAULT_PROFILE_IMAGE = "/images/default-icon.jpg";

        public FileService(
            ILogger<FileService> logger,
            IMinioService minioService)
        {
            _logger = logger;
            _minioService = minioService;
        }

        public async Task<string> SaveProfileImageAsync(IFormFile file, string oldImagePath = null)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return null;

                var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png" };
                if (!allowedTypes.Contains(file.ContentType))
                {
                    throw new InvalidOperationException("Invalid file type. Only JPEG and PNG files are allowed.");
                }

                if (!string.IsNullOrEmpty(oldImagePath) && oldImagePath != DEFAULT_PROFILE_IMAGE)
                {
                    await DeleteFileAsync(oldImagePath);
                }

                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";

                using (var stream = file.OpenReadStream())
                {
                    var url = await _minioService.SaveImageAsync(stream, uniqueFileName, file.ContentType);
                    _logger.LogInformation("Successfully saved profile image to MinIO: {Url}", url);
                    return url;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving profile image");
                throw;
            }
        }

        public async Task DeleteFileAsync(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath) || filePath == DEFAULT_PROFILE_IMAGE)
                    return;

                string fileName;
                
                if (filePath.StartsWith("http://") || filePath.StartsWith("https://"))
                {
                    try
                    {
                        var uri = new Uri(filePath);
                        fileName = Path.GetFileName(uri.LocalPath);
                    }
                    catch
                    {
                        _logger.LogWarning("Invalid URL format for file path: {FilePath}", filePath);
                        return;
                    }
                }
                else
                {
                    fileName = Path.GetFileName(filePath.TrimStart('/'));
                }

                if (string.IsNullOrEmpty(fileName))
                {
                    _logger.LogWarning("Could not extract filename from path: {FilePath}", filePath);
                    return;
                }

                try
                {
                    await _minioService.DeleteFileAsync(_minioService.ImagesBucketName, fileName);
                    _logger.LogInformation("Successfully deleted file from MinIO: {FilePath}", filePath);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to delete file from MinIO: {FilePath}", filePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file: {FilePath}", filePath);
            }
        }
    }
} 