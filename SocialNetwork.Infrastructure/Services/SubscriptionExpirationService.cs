using Elomoas.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Elomoas.Infrastructure.Services
{
    public class SubscriptionExpirationService : BackgroundService
    {
        private readonly ILogger<SubscriptionExpirationService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1);

        public SubscriptionExpirationService(
            ILogger<SubscriptionExpirationService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Checking for expired course subscriptions at: {time}", DateTimeOffset.Now);

                    try
                    {
                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            var courseSubscriptionService = scope.ServiceProvider.GetRequiredService<ICourseSubscriptionService>();
                            await courseSubscriptionService.CheckAndUpdateExpiredSubscriptionsAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error occurred while checking course subscriptions");
                    }

                    // Wait for 1 hour before next check
                    await Task.Delay(_checkInterval, stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                // Log graceful shutdown
                _logger.LogInformation("Subscription expiration service is shutting down");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fatal error in subscription expiration service");
                throw; // Re-throw fatal errors
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Subscription expiration service is stopping");
            await base.StopAsync(cancellationToken);
        }
    }
} 