using MediatR;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Application.Features.Courses.Dto;

namespace Elomoas.Application.Features.Courses.Query;

public class GetCourseSubscriptionInfoQueryHandler : IRequestHandler<GetCourseSubscriptionInfoQuery, SubscriptionInfoDto?>
{
    private readonly ICourseSubscriptionRepository _subscriptionRepository;
    public GetCourseSubscriptionInfoQueryHandler(ICourseSubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }
    public async Task<SubscriptionInfoDto?> Handle(GetCourseSubscriptionInfoQuery request, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionRepository.GetSubscription(request.AppUserId, request.CourseId);
        if (subscription == null) return null;
        return new SubscriptionInfoDto
        {
            DurationInMonths = subscription.DurationInMonths,
            SubscriptionPrice = subscription.SubscriptionPrice,
            ExpirationDate = subscription.ExpirationDate
        };
    }
} 