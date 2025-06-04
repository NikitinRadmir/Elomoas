using MediatR;
using Microsoft.Extensions.Logging;
using Elomoas.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Elomoas.Application.Features.Friends.Commands.SendFriendRequest
{
    public class SendFriendRequestCommandHandler : IRequestHandler<SendFriendRequestCommand, bool>
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly ILogger<SendFriendRequestCommandHandler> _logger;

        public SendFriendRequestCommandHandler(
            IFriendshipRepository friendshipRepository,
            ILogger<SendFriendRequestCommandHandler> logger)
        {
            _friendshipRepository = friendshipRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(SendFriendRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _friendshipRepository.SendFriendRequestAsync(request.UserId, request.FriendId);
                if (result)
                {
                    _logger.LogInformation("Friend request sent from {UserId} to {FriendId}", request.UserId, request.FriendId);
                }
                else
                {
                    _logger.LogWarning("Failed to send friend request from {UserId} to {FriendId}", request.UserId, request.FriendId);
                }
                return result;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error sending friend request from {UserId} to {FriendId}", request.UserId, request.FriendId);
                return false;
            }
        }
    }
} 