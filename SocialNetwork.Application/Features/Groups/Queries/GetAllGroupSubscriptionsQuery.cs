using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Elomoas.Application.Features.Groups.Queries;

public record GetAllGroupSubscriptionsQuery : IRequest<IEnumerable<GroupSubscription>>;

public class GetAllGroupSubscriptionsQueryHandler : IRequestHandler<GetAllGroupSubscriptionsQuery, IEnumerable<GroupSubscription>>
{
    private readonly IGroupSubscriptionService _subscriptionService;

    public GetAllGroupSubscriptionsQueryHandler(IGroupSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<IEnumerable<GroupSubscription>> Handle(GetAllGroupSubscriptionsQuery request, CancellationToken cancellationToken)
    {
        return await _subscriptionService.GetAllGroupSubscriptionsAsync();
    }
} 