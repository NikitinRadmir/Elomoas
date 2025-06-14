using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Elomoas.Infrastructure.Services;
using System.Text;
using Elomoas.Application.Interfaces.Services;

namespace Elomoas.Logging;

public class MinioLoggerProvider : ILoggerProvider
{
    private readonly IMinioService _minioService;
    private readonly ConcurrentDictionary<string, MinioLogger> _loggers = new();
    private readonly ConcurrentQueue<string> _logBuffer = new();
    private readonly Timer _flushTimer;
    private readonly object _lockObject = new();
    private bool _isDisposed;

    public MinioLoggerProvider(IMinioService minioService)
    {
        _minioService = minioService;
        _flushTimer = new Timer(FlushLogs, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
    }

    private async void FlushLogs(object? state)
    {
        if (_isDisposed || _logBuffer.IsEmpty) return;

        try
        {
            var currentDate = DateTime.Now.ToString("yyyyMMdd");
            var fileName = $"{currentDate}.log";
            var logs = new StringBuilder();

            while (_logBuffer.TryDequeue(out var logEntry))
            {
                logs.AppendLine(logEntry);
            }

            if (logs.Length > 0)
            {
                try
                {
                    using var existingLogs = await _minioService.GetFileAsync(_minioService.LogsBucketName, fileName);
                    using var reader = new StreamReader(existingLogs);
                    logs.Insert(0, await reader.ReadToEndAsync());
                }
                catch
                {
                }

                await _minioService.SaveLogAsync(logs.ToString(), fileName);
            }
        }
        catch
        {
        }
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => new MinioLogger(name, _logBuffer));
    }

    public void Dispose()
    {
        if (_isDisposed) return;
        
        _isDisposed = true;
        _flushTimer.Dispose();
        
        FlushLogs(null);
        
        _loggers.Clear();
    }
}

public class MinioLogger : ILogger
{
    private readonly string _categoryName;
    private readonly ConcurrentQueue<string> _logBuffer;

    public MinioLogger(string categoryName, ConcurrentQueue<string> logBuffer)
    {
        _categoryName = categoryName;
        _logBuffer = logBuffer;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        var formattedMessage = formatter(state, exception);
        var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{logLevel}] {_categoryName}: {formattedMessage}";
        if (exception != null)
        {
            logEntry += $"\n{exception}";
        }

        _logBuffer.Enqueue(logEntry);
    }
} 