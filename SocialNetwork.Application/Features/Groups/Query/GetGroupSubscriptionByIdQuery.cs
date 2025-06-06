using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Groups.Query;

public record GetGroupSubscriptionByIdQuery(int Id) : IRequest<GroupSubscription>;

public class GetGroupSubscriptionByIdQueryHandler : IRequestHandler<GetGroupSubscriptionByIdQuery, GroupSubscription>
{
    private readonly IGroupSubscriptionService _subscriptionService;

    public GetGroupSubscriptionByIdQueryHandler(IGroupSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<GroupSubscription> Handle(GetGroupSubscriptionByIdQuery request, CancellationToken cancellationToken)
    {
        return await _subscriptionService.GetSubscriptionByIdAsync(request.Id);
    }
}