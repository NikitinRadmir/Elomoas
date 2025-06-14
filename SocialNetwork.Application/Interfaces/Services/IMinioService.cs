using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elomoas.Application.Interfaces.Services
{
    public interface IMinioService
    {
        string LogsBucketName { get; }
        string ImagesBucketName { get; }
        Task EnsureBucketExists(string bucketName);
        Task SaveLogAsync(string logMessage, string fileName);
        Task<string> SaveImageAsync(Stream imageStream, string fileName, string contentType);
        Task<Stream> GetFileAsync(string bucketName, string fileName);
        Task DeleteFileAsync(string bucketName, string fileName);
    }

}
