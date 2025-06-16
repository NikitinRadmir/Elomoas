using Elomoas.Application.Features.CourseSubscriptions.Dtos;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Elomoas.Application.Features.CourseSubscriptions.Queries;

public class GetAllSubscriptionsQueryHandler : IRequestHandler<GetAllSubscriptionsQuery, IEnumerable<CourseSubscriptionDto>>
{
    private readonly ICourseSubscriptionService _subscriptionService;

    public GetAllSubscriptionsQueryHandler(ICourseSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<IEnumerable<CourseSubscriptionDto>> Handle(GetAllSubscriptionsQuery request, CancellationToken cancellationToken)
    {
        var data = await _subscriptionService.GetAllCourseSubscriptionsAsync();
        var result = data.Select(x => new CourseSubscriptionDto
        {
            Id = x.Id,
            UserId = x.UserId,
            CourseId = x.CourseId,
            ExpirationDate = x.ExpirationDate,
            SubscriptionPrice = x.SubscriptionPrice,
            DurationInMonths = x.DurationInMonths,
        }).ToList();

        return result;
    }
} 