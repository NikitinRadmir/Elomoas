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
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<FileService> _logger;
        private const string PROFILE_IMAGES_FOLDER = "profile-images";

        public FileService(
            IWebHostEnvironment webHostEnvironment,
            ILogger<FileService> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task<string> SaveProfileImageAsync(IFormFile file, string oldImagePath = null)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return null;

                // Validate file type
                var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png" };
                if (!allowedTypes.Contains(file.ContentType))
                {
                    throw new InvalidOperationException("Invalid file type. Only JPEG and PNG files are allowed.");
                }

                // Create the profile images directory if it doesn't exist
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, PROFILE_IMAGES_FOLDER);
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Delete old image if it exists
                if (!string.IsNullOrEmpty(oldImagePath))
                {
                    await DeleteFileAsync(oldImagePath);
                }

                // Generate unique filename
                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the new file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                _logger.LogInformation("Successfully saved profile image: {FilePath}", filePath);

                // Return the relative path
                return Path.Combine("/", PROFILE_IMAGES_FOLDER, uniqueFileName);
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
                if (string.IsNullOrEmpty(filePath))
                    return;

                // Convert relative path to absolute path
                var absolutePath = Path.Combine(_webHostEnvironment.WebRootPath, filePath.TrimStart('/'));

                if (File.Exists(absolutePath))
                {
                    await Task.Run(() => File.Delete(absolutePath));
                    _logger.LogInformation("Successfully deleted file: {FilePath}", absolutePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file: {FilePath}", filePath);
                throw;
            }
        }
    }
} 