using Elomoas.Application.Interfaces.Services;
using Elomoas.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elomoas.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<ICourseSubscriptionService, CourseSubscriptionService>();
        services.AddScoped<IGroupService, GroupService>();

        return services;
    }
} 