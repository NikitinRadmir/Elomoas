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
        Task<GroupSubscription> CreateSubscriptionAsync(GroupSubscription subscription);
        Task<bool> UpdateSubscriptionAsync(GroupSubscription subscription);
        Task<bool> DeleteSubscriptionAsync(int id);
    }
}
