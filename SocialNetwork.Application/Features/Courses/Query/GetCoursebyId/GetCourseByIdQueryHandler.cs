using System;
using System.Threading.Tasks;
using Elomoas.Application.Features.Courses.Query;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Application.Interfaces.Services;
using MediatR;

namespace Elomoas.Application.Features.Courses.Query.GetCourseById;

public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, CourseDto>
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICourseSubscriptionRepository _subscriptionRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetCourseByIdQueryHandler(
        ICourseRepository courseRepository,
        ICourseSubscriptionRepository subscriptionRepository,
        ICurrentUserService currentUserService)
    {
        _courseRepository = courseRepository;
        _subscriptionRepository = subscriptionRepository;
        _currentUserService = currentUserService;
    }

    public async Task<CourseDto> Handle(GetCourseByIdQuery query, CancellationToken ct)
    {
        var data = await _courseRepository.GetCourseById(query.id);
        
        if (data == null)
            return null;

        var result = new CourseDto
        {
            Id = data.Id,
            Name = data.Name,
            Description = data.Description,
            Img = data.Img,
            Price = data.Price,
            PL = data.PL,
            Video = data.Video,
            Learn = data.Learn,
            IsCurrentUserSubscribed = false
        };

        var currentUser = await _currentUserService.GetCurrentAppUserAsync();
        if (currentUser != null)
        {
            result.IsCurrentUserSubscribed = await _subscriptionRepository.IsSubscribed(currentUser.Id, query.id);

            if (result.IsCurrentUserSubscribed)
            {
                var subscription = await _subscriptionRepository.GetSubscription(currentUser.Id, query.id);
                if (subscription != null)
                {
                    result.SubscriptionInfo = new SubscriptionInfoDto
                    {
                        DurationInMonths = subscription.DurationInMonths,
                        SubscriptionPrice = subscription.SubscriptionPrice,
                        ExpirationDate = subscription.ExpirationDate
                    };
                }
            }
        }

        return result;
    }
}
