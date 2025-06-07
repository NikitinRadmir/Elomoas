using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using MediatR;

namespace Elomoas.Application.Features.FriendHub.Commands.NotifyFriendRequestReceived;

public class NotifyFriendRequestReceivedCommandHandler : IRequestHandler<NotifyFriendRequestReceivedCommand>
{
    private readonly IFriendHubService _friendHubService;

    public NotifyFriendRequestReceivedCommandHandler(IFriendHubService friendHubService)
    {
        _friendHubService = friendHubService;
    }

    public async Task Handle(NotifyFriendRequestReceivedCommand request, CancellationToken cancellationToken)
    {
        await _friendHubService.NotifyFriendRequestReceived(request.TargetUserId, request.Sender);
    }
} 