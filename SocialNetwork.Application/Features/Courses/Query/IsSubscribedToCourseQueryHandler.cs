using MediatR;
using Elomoas.Application.Interfaces.Repositories;

namespace Elomoas.Application.Features.Courses.Query;

public class IsSubscribedToCourseQueryHandler : IRequestHandler<IsSubscribedToCourseQuery, bool>
{
    private readonly ICourseSubscriptionRepository _subscriptionRepository;
    public IsSubscribedToCourseQueryHandler(ICourseSubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }
    public async Task<bool> Handle(IsSubscribedToCourseQuery request, CancellationToken cancellationToken)
    {
        return await _subscriptionRepository.IsSubscribed(request.AppUserId, request.CourseId);
    }
} 