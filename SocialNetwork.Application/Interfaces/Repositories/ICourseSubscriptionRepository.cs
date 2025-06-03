using System.Threading.Tasks;
using Elomoas.Domain.Entities;
using System.Collections.Generic;

namespace Elomoas.Application.Interfaces.Repositories
{
    public interface ICourseSubscriptionRepository
    {
        Task<bool> IsSubscribed(int userId, int courseId);
        Task Subscribe(int userId, int courseId, int durationInMonths);
        Task Unsubscribe(int userId, int courseId);
        Task<CourseSubscription> GetSubscription(int userId, int courseId);
        Task<IEnumerable<CourseSubscription>> GetExpiredSubscriptions();
        Task DeleteAsync(CourseSubscription subscription);
    }
} 