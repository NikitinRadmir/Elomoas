using MediatR;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Application.Interfaces.Services;

namespace Elomoas.Application.Features.Groups.Commands.SubscribeToGroup;

public class SubscribeToGroupCommandHandler : IRequestHandler<SubscribeToGroupCommand, bool>
{
    private readonly IGroupSubscriptionRepository _subscriptionRepository;
    private readonly ICurrentUserService _currentUserService;

    public SubscribeToGroupCommandHandler(
        IGroupSubscriptionRepository subscriptionRepository,
        ICurrentUserService currentUserService)
    {
        _subscriptionRepository = subscriptionRepository;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(SubscribeToGroupCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (!userId.HasValue)
            return false;

        await _subscriptionRepository.Subscribe(userId.Value, request.GroupId);
        return true;
    }
} 