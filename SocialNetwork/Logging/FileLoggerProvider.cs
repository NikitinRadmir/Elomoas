using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Elomoas.Logging;

public class FileLoggerProvider : ILoggerProvider
{
    private readonly string _path;
    private readonly ConcurrentDictionary<string, FileLogger> _loggers = new();

    public FileLoggerProvider(string path)
    {
        _path = path;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => new FileLogger(name, _path));
    }

    public void Dispose()
    {
        _loggers.Clear();
    }
}

public class FileLogger : ILogger
{
    private readonly string _categoryName;
    private readonly string _path;
    private static readonly object Lock = new();

    public FileLogger(string categoryName, string path)
    {
        _categoryName = categoryName;
        _path = path;
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

        var formattedPath = _path.Replace("{Date}", DateTime.Now.ToString("yyyyMMdd"));
        var directory = Path.GetDirectoryName(formattedPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var formattedMessage = formatter(state, exception);
        var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{logLevel}] {_categoryName}: {formattedMessage}";
        if (exception != null)
        {
            logEntry += $"\n{exception}";
        }

        lock (Lock)
        {
            File.AppendAllText(formattedPath, logEntry + Environment.NewLine);
        }
    }
} 