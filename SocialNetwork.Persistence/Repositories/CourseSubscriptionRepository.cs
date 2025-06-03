using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Elomoas.Persistence.Repositories
{
    public class CourseSubscriptionRepository : ICourseSubscriptionRepository
    {
        private readonly IGenericRepository<CourseSubscription> _repository;
        private readonly IGenericRepository<Course> _courseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CourseSubscriptionRepository(
            IGenericRepository<CourseSubscription> repository,
            IGenericRepository<Course> courseRepository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _courseRepository = courseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> IsSubscribed(int userId, int courseId)
        {
            return await _repository.Entities
                .AnyAsync(x => x.UserId == userId && x.CourseId == courseId);
        }

        public async Task<CourseSubscription> GetSubscription(int userId, int courseId)
        {
            return await _repository.Entities
                .FirstOrDefaultAsync(x => x.UserId == userId && x.CourseId == courseId);
        }

        public async Task Subscribe(int userId, int courseId, int durationInMonths)
        {
            if (!await IsSubscribed(userId, courseId))
            {
                var course = await _courseRepository.Entities
                    .FirstOrDefaultAsync(c => c.Id == courseId);

                if (course == null)
                    throw new Exception("Course not found");

                decimal discountPercent = 0;
                switch (durationInMonths)
                {
                    case 3:
                        discountPercent = 10;
                        break;
                    case 6:
                        discountPercent = 20;
                        break;
                    case 12:
                        discountPercent = 30;
                        break;
                }

                var basePrice = course.Price;
                var discount = basePrice * (discountPercent / 100m);
                var finalPrice = (basePrice - discount) * durationInMonths;

                var subscription = new CourseSubscription
                {
                    UserId = userId,
                    CourseId = courseId,
                    SubscriptionPrice = finalPrice,
                    DurationInMonths = durationInMonths,
                    ExpirationDate = DateTime.UtcNow.AddMonths(durationInMonths)
                };
                
                await _repository.AddAsync(subscription);
                await _unitOfWork.Save(CancellationToken.None);
            }
        }

        public async Task Unsubscribe(int userId, int courseId)
        {
            var subscription = await GetSubscription(userId, courseId);
            
            if (subscription != null)
            {
                await _repository.DeleteAsync(subscription);
                await _unitOfWork.Save(CancellationToken.None);
            }
        }
    }
} 