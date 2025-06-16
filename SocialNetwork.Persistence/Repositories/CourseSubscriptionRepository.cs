using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Elomoas.Persistence.Repositories
{
    public class CourseSubscriptionRepository : ICourseSubscriptionRepository
    {
        private readonly IGenericRepository<CourseSubscription> _repository;
        private readonly IGenericRepository<Course> _courseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CourseSubscriptionRepository> _logger;

        public CourseSubscriptionRepository(
            IGenericRepository<CourseSubscription> repository,
            IGenericRepository<Course> courseRepository,
            IUnitOfWork unitOfWork,
            ILogger<CourseSubscriptionRepository> logger)
        {
            _repository = repository;
            _courseRepository = courseRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> IsSubscribed(int userId, int courseId)
        {
            var now = DateTime.UtcNow;
            return await _repository.Entities
                .AnyAsync(x => x.UserId == userId && x.CourseId == courseId && x.ExpirationDate > now);
        }

        public async Task<CourseSubscription> GetSubscription(int userId, int courseId)
        {
            var now = DateTime.UtcNow;
            return await _repository.Entities
                .Include(x => x.Course)
                .Include(x => x.User)
                .Where(x => x.UserId == userId && x.CourseId == courseId && x.ExpirationDate > now)
                .OrderByDescending(x => x.ExpirationDate)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CourseSubscription>> GetExpiredSubscriptions()
        {
            var now = DateTime.UtcNow;
            return await _repository.Entities
                .Include(x => x.Course)
                .Where(x => x.ExpirationDate <= now)
                .ToListAsync();
        }

        public async Task DeleteAsync(CourseSubscription subscription)
        {
            await _repository.DeleteAsync(subscription);
        }

        public async Task Subscribe(int userId, int courseId, int durationInMonths)
        {
            _logger.LogInformation("Starting subscription process for user {UserId} to course {CourseId} for {Duration} months", 
                userId, courseId, durationInMonths);

            if (!await IsSubscribed(userId, courseId))
            {
                var course = await _courseRepository.Entities
                    .FirstOrDefaultAsync(c => c.Id == courseId);

                if (course == null)
                {
                    _logger.LogError("Course {CourseId} not found", courseId);
                    throw new Exception("Course not found");
                }

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

                _logger.LogInformation(
                    "Calculated subscription details: BasePrice={BasePrice}, Discount={Discount}%, FinalPrice={FinalPrice}", 
                    basePrice, discountPercent, finalPrice);

                var subscription = new CourseSubscription
                {
                    UserId = userId,
                    CourseId = courseId,
                    SubscriptionPrice = finalPrice,
                    DurationInMonths = durationInMonths,
                    ExpirationDate = DateTime.UtcNow.AddMonths(durationInMonths)
                };

                _logger.LogInformation(
                    "Created subscription object: Price={Price}, Duration={Duration}, ExpirationDate={ExpirationDate}",
                    subscription.SubscriptionPrice,
                    subscription.DurationInMonths,
                    subscription.ExpirationDate);
                
                await _repository.AddAsync(subscription);
                await _unitOfWork.Save(CancellationToken.None);

                _logger.LogInformation("Successfully saved subscription to database");
            }
            else
            {
                _logger.LogInformation("User {UserId} is already subscribed to course {CourseId}", userId, courseId);
            }
        }

        public async Task Unsubscribe(int userId, int courseId)
        {
            var subscription = await GetSubscription(userId, courseId);
            
            if (subscription != null)
            {
                await DeleteAsync(subscription);
                await _unitOfWork.Save(CancellationToken.None);
            }
        }
    }
} 