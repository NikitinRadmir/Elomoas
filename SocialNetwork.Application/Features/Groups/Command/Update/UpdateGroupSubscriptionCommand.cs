using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Groups.Commands;

public record UpdateGroupSubscriptionCommand : IRequest<bool>
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public int GroupId { get; init; }
}

public class UpdateGroupSubscriptionCommandHandler : IRequestHandler<UpdateGroupSubscriptionCommand, bool>
{
    private readonly IGroupSubscriptionService _subscriptionService;

    public UpdateGroupSubscriptionCommandHandler(IGroupSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<bool> Handle(UpdateGroupSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var subscription = new GroupSubscription
        {
            Id = request.Id,
            UserId = request.UserId,
            GroupId = request.GroupId
        };

        return await _subscriptionService.UpdateSubscriptionAsync(subscription);
    }
} 