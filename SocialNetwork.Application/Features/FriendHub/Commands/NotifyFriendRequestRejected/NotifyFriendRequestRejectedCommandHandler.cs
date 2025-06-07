using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using MediatR;

namespace Elomoas.Application.Features.FriendHub.Commands.NotifyFriendRequestRejected;

public class NotifyFriendRequestRejectedCommandHandler : IRequestHandler<NotifyFriendRequestRejectedCommand>
{
    private readonly IFriendHubService _friendHubService;

    public NotifyFriendRequestRejectedCommandHandler(IFriendHubService friendHubService)
    {
        _friendHubService = friendHubService;
    }

    public async Task Handle(NotifyFriendRequestRejectedCommand request, CancellationToken cancellationToken)
    {
        await _friendHubService.NotifyFriendRequestRejected(request.TargetUserId, request.Rejecter);
    }
} 