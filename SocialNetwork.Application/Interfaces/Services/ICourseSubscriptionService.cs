using Elomoas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elomoas.Application.Interfaces.Services;

public interface ICourseSubscriptionService
{
    Task<IEnumerable<CourseSubscription>> GetAllCourseSubscriptionsAsync();
    Task<CourseSubscription?> GetSubscriptionByIdAsync(int id);
    Task<CourseSubscription> CreateSubscriptionAsync(CourseSubscription subscription);
    Task<bool> UpdateSubscriptionAsync(CourseSubscription subscription);
    Task<bool> DeleteSubscriptionAsync(int id);
} 