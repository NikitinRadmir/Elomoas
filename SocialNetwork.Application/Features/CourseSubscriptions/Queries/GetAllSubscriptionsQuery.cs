using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Elomoas.Application.Features.CourseSubscriptions.Queries;

public record GetAllSubscriptionsQuery : IRequest<IEnumerable<CourseSubscription>>;

public class GetAllSubscriptionsQueryHandler : IRequestHandler<GetAllSubscriptionsQuery, IEnumerable<CourseSubscription>>
{
    private readonly ICourseSubscriptionService _subscriptionService;

    public GetAllSubscriptionsQueryHandler(ICourseSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<IEnumerable<CourseSubscription>> Handle(GetAllSubscriptionsQuery request, CancellationToken cancellationToken)
    {
        return await _subscriptionService.GetAllCourseSubscriptionsAsync();
    }
} 