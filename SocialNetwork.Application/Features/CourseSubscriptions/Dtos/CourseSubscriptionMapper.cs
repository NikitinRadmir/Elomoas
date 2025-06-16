using System.Collections.Generic;
using System.Linq;
using Elomoas.Domain.Entities;
using Elomoas.Application.Features.Courses.Query;
using Elomoas.Application.Features.AppUsers.Query;

namespace Elomoas.Application.Features.CourseSubscriptions.Dtos;

public static class CourseSubscriptionMapper
{
    public static CourseSubscriptionDto ToDto(this CourseSubscription subscription)
    {
        if (subscription == null) return null;

        return new CourseSubscriptionDto
        {
            Id = subscription.Id,
            UserId = subscription.UserId,
            CourseId = subscription.CourseId,
            SubscriptionPrice = subscription.SubscriptionPrice,
            DurationInMonths = subscription.DurationInMonths,
            ExpirationDate = subscription.ExpirationDate
        };
    }

    public static IEnumerable<CourseSubscriptionDto> ToDtos(this IEnumerable<CourseSubscription> subscriptions)
    {
        if (subscriptions == null) return Enumerable.Empty<CourseSubscriptionDto>();

        return subscriptions.Select(s => s.ToDto());
    }

    public static CourseSubscription ToEntity(this CourseSubscriptionDto dto)
    {
        if (dto == null) return null;

        return new CourseSubscription
        {
            Id = dto.Id,
            UserId = dto.UserId,
            CourseId = dto.CourseId,
            SubscriptionPrice = dto.SubscriptionPrice,
            DurationInMonths = dto.DurationInMonths,
            ExpirationDate = dto.ExpirationDate,
        };
    }
} 