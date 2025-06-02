using Microsoft.AspNetCore.Http.Extensions;
using System.Security.Claims;

namespace Elomoas.Middleware;

public class UserActivityLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UserActivityLoggingMiddleware> _logger;

    public UserActivityLoggingMiddleware(RequestDelegate next, ILogger<UserActivityLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmail = context.User.FindFirst(ClaimTypes.Email)?.Value;

            if (context.Request.Method == "POST" || 
                context.Request.Method == "PUT" || 
                context.Request.Method == "DELETE")
            {
                _logger.LogInformation(
                    "User Activity: {UserEmail} (ID: {UserId}) performed {RequestMethod} on {RequestPath}",
                    userEmail,
                    userId,
                    context.Request.Method,
                    context.Request.Path);
            }
        }

        await _next(context);
    }
} 