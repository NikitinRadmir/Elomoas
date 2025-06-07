using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Groups.Commands;

public record CreateGroupSubscriptionCommand : IRequest<GroupSubscription>
{
    public int UserId { get; init; }
    public int GroupId { get; init; }
}

public class CreateGroupSubscriptionCommandHandler : IRequestHandler<CreateGroupSubscriptionCommand, GroupSubscription>
{
    private readonly IGroupSubscriptionService _subscriptionService;

    public CreateGroupSubscriptionCommandHandler(IGroupSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<GroupSubscription> Handle(CreateGroupSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var subscription = new GroupSubscription
        {
            UserId = request.UserId,
            GroupId = request.GroupId
        };

        return await _subscriptionService.CreateSubscriptionAsync(subscription);
    }
} 