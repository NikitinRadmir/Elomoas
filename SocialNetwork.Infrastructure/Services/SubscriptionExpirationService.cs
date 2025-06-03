using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elomoas.Infrastructure.Services
{
    public class SubscriptionExpirationService : BackgroundService, ISubscriptionExpirationService
    {
        private readonly ILogger<SubscriptionExpirationService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1);

        public SubscriptionExpirationService(
            ILogger<SubscriptionExpirationService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Subscription Expiration Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessExpiredSubscriptionsAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing expired subscriptions");
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }

        public async Task ProcessExpiredSubscriptionsAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Checking for expired subscriptions at: {time}", DateTimeOffset.Now);

            using (var scope = _serviceProvider.CreateScope())
            {
                var subscriptionRepo = scope.ServiceProvider.GetRequiredService<ICourseSubscriptionRepository>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                try
                {
                    var expiredSubscriptions = await subscriptionRepo.GetExpiredSubscriptions();
                    
                    if (!expiredSubscriptions.Any())
                    {
                        _logger.LogInformation("No expired subscriptions found.");
                        return;
                    }

                    _logger.LogInformation("Found {count} expired subscriptions", expiredSubscriptions.Count());

                    foreach (var subscription in expiredSubscriptions)
                    {
                        try
                        {
                            await subscriptionRepo.DeleteAsync(subscription);
                            _logger.LogInformation(
                                "Successfully unsubscribed user {userId} from course {courseId}. Subscription expired at {expirationDate}",
                                subscription.UserId,
                                subscription.CourseId,
                                subscription.ExpirationDate);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex,
                                "Error unsubscribing user {userId} from course {courseId}",
                                subscription.UserId,
                                subscription.CourseId);
                        }
                    }

                    await unitOfWork.Save(cancellationToken);
                    _logger.LogInformation("Finished processing expired subscriptions");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing expired subscriptions batch");
                    throw;
                }
            }
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Subscription Expiration Service is starting.");
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Subscription Expiration Service is stopping.");
            await base.StopAsync(cancellationToken);
        }
    }
} 