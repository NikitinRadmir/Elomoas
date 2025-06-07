using Elomoas.Application.Interfaces.Services;
using MediatR;


namespace SocialNetwork.Application.Features.FriendHub.Commands.NotifyFriendRemoved;

public class NotifyFriendRemovedCommandHandler : IRequestHandler<NotifyFriendRemovedCommand>
{
    private readonly IFriendHubService _friendHubService;

    public NotifyFriendRemovedCommandHandler(IFriendHubService friendHubService)
    {
        _friendHubService = friendHubService;
    }

    public async Task Handle(NotifyFriendRemovedCommand request, CancellationToken cancellationToken)
    {
        await _friendHubService.NotifyFriendRemovedAsync(request.FriendId, request.RemoverId, request.RemoverName);
    }
} 