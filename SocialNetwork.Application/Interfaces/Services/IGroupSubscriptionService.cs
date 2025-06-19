using Elomoas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elomoas.Application.Interfaces.Services
{
    public interface IGroupSubscriptionService
    {
        Task<IEnumerable<GroupSubscription>> GetAllGroupSubscriptionsAsync();
        Task<GroupSubscription?> GetSubscriptionByIdAsync(int id);
        Task<bool> CreateSubscriptionAsync(GroupSubscription subscription);
        Task<bool> UpdateSubscriptionAsync(GroupSubscription subscription);
        Task<bool> DeleteSubscriptionAsync(int id);
        Task<bool> CreateSubscriptionAsync(int userId, int groupId);
        Task<bool> UpdateSubscriptionAsync(int id, int userId, int groupId);
    }
}
