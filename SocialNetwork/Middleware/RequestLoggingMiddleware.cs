using System.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;

namespace Elomoas.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            
            _logger.LogInformation(
                "Request Started {RequestMethod} {RequestPath} {QueryString}",
                context.Request.Method,
                context.Request.Path,
                context.Request.QueryString);

            await _next(context);

            stopwatch.Stop();

            _logger.LogInformation(
                "Request Completed {RequestMethod} {RequestPath} {StatusCode} in {Elapsed:0.0000}ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.Elapsed.TotalMilliseconds);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Request Failed {RequestMethod} {RequestPath}",
                context.Request.Method,
                context.Request.Path);
            throw;
        }
    }
} 