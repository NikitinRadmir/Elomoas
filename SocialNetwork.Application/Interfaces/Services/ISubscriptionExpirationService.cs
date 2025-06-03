using System.Threading;
using System.Threading.Tasks;

namespace Elomoas.Application.Interfaces.Services
{
    public interface ISubscriptionExpirationService
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
        Task ProcessExpiredSubscriptionsAsync(CancellationToken cancellationToken);
    }
}