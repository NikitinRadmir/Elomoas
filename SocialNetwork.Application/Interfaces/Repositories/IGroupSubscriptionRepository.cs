using Elomoas.Domain.Entities;
using System.Threading.Tasks;

namespace Elomoas.Application.Interfaces.Repositories
{
    public interface IGroupSubscriptionRepository
    {
        Task<bool> IsSubscribed(int userId, int groupId);
        Task Subscribe(int userId, int groupId);
        Task Unsubscribe(int userId, int groupId);
    }
} 