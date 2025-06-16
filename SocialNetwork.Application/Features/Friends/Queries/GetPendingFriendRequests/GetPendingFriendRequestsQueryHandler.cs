using MediatR;
using Microsoft.Extensions.Logging;
using Elomoas.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Features.Friends.Dtos;
using System.Linq;

namespace Elomoas.Application.Features.Friends.Queries.GetPendingFriendRequests
{
    public class GetPendingFriendRequestsQueryHandler : IRequestHandler<GetPendingFriendRequestsQuery, IEnumerable<FriendshipDto>>
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly ILogger<GetPendingFriendRequestsQueryHandler> _logger;

        public GetPendingFriendRequestsQueryHandler(
            IFriendshipRepository friendshipRepository,
            ILogger<GetPendingFriendRequestsQueryHandler> logger)
        {
            _friendshipRepository = friendshipRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<FriendshipDto>> Handle(GetPendingFriendRequestsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var pendingRequests = await _friendshipRepository.GetPendingFriendshipsAsync(request.UserId);
                if (pendingRequests == null || !pendingRequests.Any())
                {
                    _logger.LogInformation("No pending friend requests found for user {UserId}", request.UserId);
                    return Enumerable.Empty<FriendshipDto>();
                }

                var dtos = pendingRequests.Select(friendship => new FriendshipDto
                {
                    UserId = friendship.UserId,
                    FriendId = friendship.FriendId,
                    Status = friendship.Status
                });

                _logger.LogInformation("Retrieved {Count} pending friend requests for user {UserId}", 
                    dtos.Count(), request.UserId);

                return dtos;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pending friend requests for user {UserId}", request.UserId);
                return Enumerable.Empty<FriendshipDto>();
            }
        }
    }
} 