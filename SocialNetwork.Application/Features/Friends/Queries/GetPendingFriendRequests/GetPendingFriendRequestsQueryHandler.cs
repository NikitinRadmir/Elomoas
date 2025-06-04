using MediatR;
using Microsoft.Extensions.Logging;
using Elomoas.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Domain.Entities;

namespace Elomoas.Application.Features.Friends.Queries.GetPendingFriendRequests
{
    public class GetPendingFriendRequestsQueryHandler : IRequestHandler<GetPendingFriendRequestsQuery, IEnumerable<Friendship>>
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

        public async Task<IEnumerable<Friendship>> Handle(GetPendingFriendRequestsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var pendingRequests = await _friendshipRepository.GetPendingFriendshipsAsync(request.UserId);
                _logger.LogInformation("Retrieved pending friend requests for user {UserId}", request.UserId);
                return pendingRequests;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pending friend requests for user {UserId}", request.UserId);
                return new List<Friendship>();
            }
        }
    }
} 