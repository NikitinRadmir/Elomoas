using MediatR;
using Microsoft.Extensions.Logging;
using Elomoas.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Elomoas.Application.Features.Friends.Commands.RemoveFriend
{
    public class RemoveFriendCommandHandler : IRequestHandler<RemoveFriendCommand, bool>
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly ILogger<RemoveFriendCommandHandler> _logger;

        public RemoveFriendCommandHandler(
            IFriendshipRepository friendshipRepository,
            ILogger<RemoveFriendCommandHandler> logger)
        {
            _friendshipRepository = friendshipRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(RemoveFriendCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _friendshipRepository.RemoveFriendAsync(request.UserId, request.FriendId);
                if (result)
                {
                    _logger.LogInformation("Friend removed: {UserId} removed {FriendId}", request.UserId, request.FriendId);
                }
                else
                {
                    _logger.LogWarning("Failed to remove friend: {UserId} -> {FriendId}", request.UserId, request.FriendId);
                }
                return result;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error removing friend: {UserId} -> {FriendId}", request.UserId, request.FriendId);
                return false;
            }
        }
    }
} 