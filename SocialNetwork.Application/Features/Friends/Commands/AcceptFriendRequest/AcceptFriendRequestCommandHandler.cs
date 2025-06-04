using MediatR;
using Microsoft.Extensions.Logging;
using Elomoas.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Elomoas.Application.Features.Friends.Commands.AcceptFriendRequest
{
    public class AcceptFriendRequestCommandHandler : IRequestHandler<AcceptFriendRequestCommand, bool>
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly ILogger<AcceptFriendRequestCommandHandler> _logger;

        public AcceptFriendRequestCommandHandler(
            IFriendshipRepository friendshipRepository,
            ILogger<AcceptFriendRequestCommandHandler> logger)
        {
            _friendshipRepository = friendshipRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(AcceptFriendRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _friendshipRepository.AcceptFriendRequestAsync(request.UserId, request.FriendId);
                if (result)
                {
                    _logger.LogInformation("Friend request accepted from {FriendId} by {UserId}", request.FriendId, request.UserId);
                }
                else
                {
                    _logger.LogWarning("Failed to accept friend request from {FriendId} by {UserId}", request.FriendId, request.UserId);
                }
                return result;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error accepting friend request from {FriendId} by {UserId}", request.FriendId, request.UserId);
                return false;
            }
        }
    }
} 