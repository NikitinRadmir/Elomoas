using System.Text;
using Minio;
using Minio.DataModel;
using Microsoft.Extensions.Options;
using Elomoas.Infrastructure.Settings;
using Minio.DataModel.Args;
using Elomoas.Application.Interfaces.Services;

namespace Elomoas.Infrastructure.Services;


public class MinioService : IMinioService
{
    private readonly IMinioClient _minioClient;
    private readonly MinioSettings _settings;

    public string LogsBucketName => _settings.LogsBucketName;
    public string ImagesBucketName => _settings.ImagesBucketName;

    public MinioService(IOptions<MinioSettings> settings)
    {
        _settings = settings.Value;
        _minioClient = new MinioClient()
            .WithEndpoint(_settings.Endpoint)
            .WithCredentials(_settings.AccessKey, _settings.SecretKey)
            .WithSSL(_settings.UseSSL)
            .Build();
    }

    public async Task EnsureBucketExists(string bucketName)
    {
        var bucketExists = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
        if (!bucketExists)
        {
            await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
            
            if (bucketName == ImagesBucketName)
            {
                var policy = $@"{{
                    ""Version"": ""2012-10-17"",
                    ""Statement"": [
                        {{
                            ""Effect"": ""Allow"",
                            ""Principal"": {{
                                ""AWS"": [""*""]
                            }},
                            ""Action"": [
                                ""s3:GetObject"",
                                ""s3:GetBucketLocation"",
                                ""s3:ListBucket""
                            ],
                            ""Resource"": [
                                ""arn:aws:s3:::{bucketName}"",
                                ""arn:aws:s3:::{bucketName}/*""
                            ]
                        }}
                    ]
                }}";

                await _minioClient.SetPolicyAsync(new SetPolicyArgs().WithBucket(bucketName).WithPolicy(policy));
            }
        }
    }

    public async Task SaveLogAsync(string logMessage, string fileName)
    {
        await EnsureBucketExists(_settings.LogsBucketName);

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(logMessage));
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_settings.LogsBucketName)
            .WithObject(fileName)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType("text/plain");

        await _minioClient.PutObjectAsync(putObjectArgs);
    }

    public async Task<string> SaveImageAsync(Stream imageStream, string fileName, string contentType)
    {
        await EnsureBucketExists(_settings.ImagesBucketName);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_settings.ImagesBucketName)
            .WithObject(fileName)
            .WithStreamData(imageStream)
            .WithObjectSize(imageStream.Length)
            .WithContentType(contentType);

        await _minioClient.PutObjectAsync(putObjectArgs);

        return $"{_settings.PublicUrl}/{_settings.ImagesBucketName}/{fileName}";
    }

    public async Task<Stream> GetFileAsync(string bucketName, string fileName)
    {
        var memoryStream = new MemoryStream();
        
        var getObjectArgs = new GetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName)
            .WithCallbackStream(stream => stream.CopyTo(memoryStream));

        await _minioClient.GetObjectAsync(getObjectArgs);
        
        memoryStream.Position = 0;
        return memoryStream;
    }

    public async Task DeleteFileAsync(string bucketName, string fileName)
    {
        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName);

        await _minioClient.RemoveObjectAsync(removeObjectArgs);
    }
} 