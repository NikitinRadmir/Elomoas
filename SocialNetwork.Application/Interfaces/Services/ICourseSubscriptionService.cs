using Elomoas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elomoas.Application.Interfaces.Services;

public interface ICourseSubscriptionService
{
    Task<IEnumerable<CourseSubscription>> GetAllCourseSubscriptionsAsync();
    Task<CourseSubscription?> GetSubscriptionByIdAsync(int id);
    Task<bool> CreateSubscriptionAsync(int userId, int courseId, decimal subscriptionPrice, int durationInMonths, DateTime expirationDate);
    Task<bool> UpdateSubscriptionAsync(int id, int userId, int courseId, decimal subscriptionPrice, int durationInMonths, DateTime expirationDate);
    Task<bool> DeleteSubscriptionAsync(int id);
    Task CheckAndUpdateExpiredSubscriptionsAsync();
} 