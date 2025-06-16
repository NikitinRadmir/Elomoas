using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MediatR;
using Elomoas.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Elomoas.Application.Features.Courses.Query.GetSubscribedCourses
{
    public class GetSubscribedCoursesQueryHandler : IRequestHandler<GetSubscribedCoursesQuery, IEnumerable<CourseDto>>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseSubscriptionRepository _subscriptionRepository;
        private readonly ILogger<GetSubscribedCoursesQueryHandler> _logger;

        public GetSubscribedCoursesQueryHandler(
            ICourseRepository courseRepository,
            ICourseSubscriptionRepository subscriptionRepository,
            ILogger<GetSubscribedCoursesQueryHandler> logger)
        {
            _courseRepository = courseRepository;
            _subscriptionRepository = subscriptionRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<CourseDto>> Handle(GetSubscribedCoursesQuery query, CancellationToken ct)
        {
            _logger.LogInformation("Getting subscribed courses for user {UserId}", query.UserId);

            var allCoursesDto = await _courseRepository.GetAllCoursesAsDto();
            var subscribedCoursesDto = new List<CourseDto>();

            foreach (var courseDto in allCoursesDto)
            {
                var isSubscribed = await _subscriptionRepository.IsSubscribed(query.UserId, courseDto.Id);
                if (isSubscribed)
                {
                    var subscription = await _subscriptionRepository.GetSubscription(query.UserId, courseDto.Id);
                    if (subscription != null)
                    {
                        courseDto.IsCurrentUserSubscribed = true;
                        courseDto.SubscriptionInfo = new SubscriptionInfoDto
                        {
                            DurationInMonths = subscription.DurationInMonths,
                            SubscriptionPrice = subscription.SubscriptionPrice,
                            ExpirationDate = subscription.ExpirationDate
                        };
                    }
                    subscribedCoursesDto.Add(courseDto);
                }
            }

            _logger.LogInformation("Found {Count} subscribed courses for user {UserId}", 
                subscribedCoursesDto.Count, query.UserId);

            return subscribedCoursesDto;
        }
    }
}