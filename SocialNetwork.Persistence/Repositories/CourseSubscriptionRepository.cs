using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Elomoas.Persistence.Repositories
{
    public class CourseSubscriptionRepository : ICourseSubscriptionRepository
    {
        private readonly IGenericRepository<CourseSubscription> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CourseSubscriptionRepository(
            IGenericRepository<CourseSubscription> repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> IsSubscribed(int userId, int courseId)
        {
            return await _repository.Entities
                .AnyAsync(x => x.UserId == userId && x.CourseId == courseId);
        }

        public async Task Subscribe(int userId, int courseId)
        {
            if (!await IsSubscribed(userId, courseId))
            {
                var subscription = new CourseSubscription
                {
                    UserId = userId,
                    CourseId = courseId
                };
                await _repository.AddAsync(subscription);
                await _unitOfWork.Save(CancellationToken.None);
            }
        }

        public async Task Unsubscribe(int userId, int courseId)
        {
            var subscription = await _repository.Entities
                .FirstOrDefaultAsync(x => x.UserId == userId && x.CourseId == courseId);
            
            if (subscription != null)
            {
                await _repository.DeleteAsync(subscription);
                await _unitOfWork.Save(CancellationToken.None);
            }
        }
    }
} 