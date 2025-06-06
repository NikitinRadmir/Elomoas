using MediatR;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Application.Interfaces.Services;

namespace Elomoas.Application.Features.Groups.Command.UnsubscribeFromGroup;

public class UnsubscribeFromGroupCommandHandler : IRequestHandler<UnsubscribeFromGroupCommand, bool>
{
    private readonly IGroupSubscriptionRepository _subscriptionRepository;
    private readonly ICurrentUserService _currentUserService;

    public UnsubscribeFromGroupCommandHandler(
        IGroupSubscriptionRepository subscriptionRepository,
        ICurrentUserService currentUserService)
    {
        _subscriptionRepository = subscriptionRepository;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(UnsubscribeFromGroupCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (!userId.HasValue)
            return false;

        await _subscriptionRepository.Unsubscribe(userId.Value, request.GroupId);
        return true;
    }
}