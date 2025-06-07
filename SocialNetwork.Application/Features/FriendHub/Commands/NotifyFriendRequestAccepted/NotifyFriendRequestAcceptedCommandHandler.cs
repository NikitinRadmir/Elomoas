using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using MediatR;

namespace Elomoas.Application.Features.FriendHub.Commands.NotifyFriendRequestAccepted;

public class NotifyFriendRequestAcceptedCommandHandler : IRequestHandler<NotifyFriendRequestAcceptedCommand>
{
    private readonly IFriendHubService _friendHubService;

    public NotifyFriendRequestAcceptedCommandHandler(IFriendHubService friendHubService)
    {
        _friendHubService = friendHubService;
    }

    public async Task Handle(NotifyFriendRequestAcceptedCommand request, CancellationToken cancellationToken)
    {
        await _friendHubService.NotifyFriendRequestAccepted(request.TargetUserId, request.Accepter);
    }
} 