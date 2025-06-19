using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;
using Elomoas.Application.Features.Groups.Query.GetSubscriptions;

namespace Elomoas.Application.Features.Groups.Queries;

public class GetGroupSubscriptionByIdQueryHandler : IRequestHandler<GetGroupSubscriptionByIdQuery, GroupSubscriptionDto>
{
    private readonly IGroupSubscriptionService _subscriptionService;

    public GetGroupSubscriptionByIdQueryHandler(IGroupSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<GroupSubscriptionDto> Handle(GetGroupSubscriptionByIdQuery request, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionService.GetSubscriptionByIdAsync(request.Id);
        if (subscription == null)
            return null;

        return new GroupSubscriptionDto
        {
            Id = subscription.Id,
            UserId = subscription.UserId,
            GroupId = subscription.GroupId,
            UserName = subscription.User?.Name ?? "Unknown",
            GroupName = subscription.Group?.Name ?? "Unknown",

        };
    }
} 