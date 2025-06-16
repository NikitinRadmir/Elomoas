using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Elomoas.Application.Features.Friends.Commands.UpdateFriendship;

public class UpdateFriendshipCommandHandler : IRequestHandler<UpdateFriendshipCommand, bool>
{
    private readonly IFriendshipService _friendshipService;
    private readonly ILogger<UpdateFriendshipCommandHandler> _logger;

    public UpdateFriendshipCommandHandler(
        IFriendshipService friendshipService,
        ILogger<UpdateFriendshipCommandHandler> logger)
    {
        _friendshipService = friendshipService;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateFriendshipCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating friendship {Id}. UserId: {UserId}, FriendId: {FriendId}, Status: {Status}",
            request.Id, request.UserId, request.FriendId, request.Status);

        return await _friendshipService.UpdateFriendshipAsync(
            request.Id,
            request.UserId,
            request.FriendId,
            request.Status);
    }
} 