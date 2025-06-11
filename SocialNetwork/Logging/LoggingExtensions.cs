using Microsoft.Extensions.Logging;
using Elomoas.Infrastructure.Services;

namespace Elomoas.Logging;

public static class LoggingExtensions
{
    public static ILoggingBuilder AddFile(this ILoggingBuilder builder, string filePath)
    {
        builder.AddProvider(new FileLoggerProvider(filePath));
        return builder;
    }

    public static ILoggingBuilder AddMinio(this ILoggingBuilder builder, IMinioService minioService)
    {
        builder.AddProvider(new MinioLoggerProvider(minioService));
        return builder;
    }
} 