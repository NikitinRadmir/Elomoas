using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Application.Interfaces.Services;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elomoas.Application.Features.Courses.Query.GetLatestCourses;

public class GetLatestCoursesQueryHandler : IRequestHandler<GetLatestCoursesQuery, IEnumerable<CourseDto>>
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICourseSubscriptionRepository _subscriptionRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetLatestCoursesQueryHandler(
        ICourseRepository courseRepository,
        ICourseSubscriptionRepository subscriptionRepository,
        ICurrentUserService currentUserService)
    {
        _courseRepository = courseRepository;
        _subscriptionRepository = subscriptionRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<CourseDto>> Handle(GetLatestCoursesQuery query, CancellationToken ct)
    {
        var data = await _courseRepository.GetAllCoursesAsync();
        var currentUser = await _currentUserService.GetCurrentAppUserAsync();

        var result = data.OrderByDescending(x => x.Id).Take(7).Select(x => new CourseDto
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