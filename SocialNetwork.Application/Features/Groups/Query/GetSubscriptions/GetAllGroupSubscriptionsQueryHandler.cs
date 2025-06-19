using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;
using Elomoas.Application.Features.Groups.Query.GetSubscriptions;
using System.Linq;

namespace Elomoas.Application.Features.Groups.Queries;

public class GetAllGroupSubscriptionsQueryHandler : IRequestHandler<GetAllGroupSubscriptionsQuery, IEnumerable<GroupSubscriptionDto>>
{
    private readonly IGroupSubscriptionService _subscriptionService;

    public GetAllGroupSubscriptionsQueryHandler(IGroupSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<IEnumerable<GroupSubscriptionDto>> Handle(GetAllGroupSubscriptionsQuery request, CancellationToken cancellationToken)
    {
        var subscriptions = await _subscriptionService.GetAllGroupSubscriptionsAsync();
        return subscriptions.Select(x => new GroupSubscriptionDto
        {
            Id = x.Id,
            UserId = x.UserId,
            GroupId = x.GroupId,
            UserName = x.User?.Name ?? "Unknown",
            GroupName = x.Group?.Name ?? "Unknown",
        });
    }
} 