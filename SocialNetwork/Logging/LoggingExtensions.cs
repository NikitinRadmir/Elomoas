using Microsoft.Extensions.Logging;

namespace Elomoas.Logging;

public static class LoggingExtensions
{
    public static ILoggingBuilder AddFile(this ILoggingBuilder builder, string filePath)
    {
        builder.AddProvider(new FileLoggerProvider(filePath));
        return builder;
    }
} 