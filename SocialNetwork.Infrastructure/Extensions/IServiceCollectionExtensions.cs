using Elomoas.Application.Interfaces.Services;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Infrastructure.Services;
using Elomoas.Persistence.Contexts;
using Elomoas.Persistence.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Elomoas.Domain.Entities;


namespace Elomoas.Infrastructure.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services)
        {
            services.AddServices();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services
                .AddTransient<IMediator, Mediator>()
                .AddTransient<ISubscriptionExpirationService, SubscriptionExpirationService>()
                .AddHostedService<SubscriptionExpirationService>()
                .AddTransient<ICurrentUserService, CurrentUserService>()
                .AddHttpContextAccessor()
                .AddTransient<IEmailService, EmailService>()
                .AddTransient<IAuthService, AuthService>()
                .AddTransient<IGroupSubscriptionService, GroupSubscriptionService>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<IFriendshipService, FriendshipService>()
                .AddTransient<IChatService, ChatService>()
                .AddTransient<ICourseService, CourseService>()
                .AddTransient<ICourseSubscriptionService, CourseSubscriptionService>()
                .AddTransient<IGroupService, GroupService>()
                .AddTransient<ISettingsService, SettingsService>()
                .AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
