using System.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;
using System.Security.Claims;

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
        var stopwatch = Stopwatch.StartNew();
        var userName = context.User?.Identity?.IsAuthenticated == true
            ? context.User.FindFirst(ClaimTypes.Email)?.Value ?? "Unknown"
            : "Anonymous";

        try
        {
            await _next(context);
            
            stopwatch.Stop();
            _logger.LogInformation(
                "{UserName} | {RequestMethod} {RequestPath} | Status: {StatusCode} | Time: {Elapsed:0.0000}ms",
                userName,
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.Elapsed.TotalMilliseconds);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(
                ex,
                "{UserName} | {RequestMethod} {RequestPath} | Failed",
                userName,
                context.Request.Method,
                context.Request.Path);
            throw;
        }
    }
} 