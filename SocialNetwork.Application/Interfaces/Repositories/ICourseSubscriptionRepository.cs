using System.Threading.Tasks;

namespace Elomoas.Application.Interfaces.Repositories
{
    public interface ICourseSubscriptionRepository
    {
        Task<bool> IsSubscribed(int userId, int courseId);
        Task Subscribe(int userId, int courseId);
        Task Unsubscribe(int userId, int courseId);
    }
} 