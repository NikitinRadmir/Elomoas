using Elomoas.Application.Features.AppUsers.Query.GetUserById;
using Elomoas.Application.Features.AppUsers.Query;
using Elomoas.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Elomoas.Application.Interfaces.Services;

namespace Elomoas.Application.Features.Courses.Query.GetAllCourses
{
    public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, IEnumerable<CourseDto>>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseSubscriptionRepository _subscriptionRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetAllCoursesQueryHandler(
            ICourseRepository courseRepository,
            ICourseSubscriptionRepository subscriptionRepository,
            ICurrentUserService currentUserService)
        {
            _courseRepository = courseRepository;
            _subscriptionRepository = subscriptionRepository;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<CourseDto>> Handle(GetAllCoursesQuery query, CancellationToken ct)
        {
            var data = await _courseRepository.GetAllCoursesAsync();
            var currentUser = await _currentUserService.GetCurrentAppUserAsync();

            var result = data.OrderBy(x => x.Id).Select(x => new CourseDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Img = x.Img,
                Price = x.Price,
                PL = x.PL,
                Video = x.Video,
                Learn = x.Learn,
                IsCurrentUserSubscribed = false
            }).ToList();

            if (currentUser != null)
            {
                foreach (var course in result)
                {
                    course.IsCurrentUserSubscribed = await _subscriptionRepository.IsSubscribed(currentUser.Id, course.Id);

                    if (course.IsCurrentUserSubscribed)
                    {
                        var subscription = await _subscriptionRepository.GetSubscription(currentUser.Id, course.Id);
                        if (subscription != null)
                        {
                            course.SubscriptionInfo = new SubscriptionInfoDto
                            {
                                DurationInMonths = subscription.DurationInMonths,
                                SubscriptionPrice = subscription.SubscriptionPrice,
                                ExpirationDate = subscription.ExpirationDate
                            };
                        }
                    }
                }
            }

            return result;
        }
    }
}
