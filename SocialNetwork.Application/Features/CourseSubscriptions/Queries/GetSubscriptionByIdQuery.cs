using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.CourseSubscriptions.Queries;

public record GetSubscriptionByIdQuery(int Id) : IRequest<CourseSubscription>;

public class GetSubscriptionByIdQueryHandler : IRequestHandler<GetSubscriptionByIdQuery, CourseSubscription>
{
    private readonly ICourseSubscriptionService _subscriptionService;

    public GetSubscriptionByIdQueryHandler(ICourseSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<CourseSubscription> Handle(GetSubscriptionByIdQuery request, CancellationToken cancellationToken)
    {
        return await _subscriptionService.GetSubscriptionByIdAsync(request.Id);
    }
} 