using MediatR;
using Microsoft.Extensions.Logging;
using Elomoas.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Elomoas.Application.Features.Friends.Commands.RejectFriendRequest
{
    public class RejectFriendRequestCommandHandler : IRequestHandler<RejectFriendRequestCommand, bool>
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly ILogger<RejectFriendRequestCommandHandler> _logger;

        public RejectFriendRequestCommandHandler(
            IFriendshipRepository friendshipRepository,
            ILogger<RejectFriendRequestCommandHandler> logger)
        {
            _friendshipRepository = friendshipRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(RejectFriendRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _friendshipRepository.RejectFriendRequestAsync(request.UserId, request.FriendId);
                if (result)
                {
                    _logger.LogInformation("Friend request rejected successfully. UserId: {UserId}, FriendId: {FriendId}", 
                        request.UserId, request.FriendId);
                }
                else
                {
                    _logger.LogWarning("Failed to reject friend request. UserId: {UserId}, FriendId: {FriendId}", 
                        request.UserId, request.FriendId);
                }
                return result;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error rejecting friend request. UserId: {UserId}, FriendId: {FriendId}", 
                    request.UserId, request.FriendId);
                return false;
            }
        }
    }
} 