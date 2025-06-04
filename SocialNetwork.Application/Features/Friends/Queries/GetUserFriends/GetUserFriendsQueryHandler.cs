using MediatR;
using Microsoft.Extensions.Logging;
using Elomoas.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Features.AppUsers.Query;
using System.Linq;
using Elomoas.Domain.Entities;

namespace Elomoas.Application.Features.Friends.Queries.GetUserFriends
{
    public class GetUserFriendsQueryHandler : IRequestHandler<GetUserFriendsQuery, IEnumerable<AppUserDto>>
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IAppUserRepository _userRepository;
        private readonly ILogger<GetUserFriendsQueryHandler> _logger;

        public GetUserFriendsQueryHandler(
            IFriendshipRepository friendshipRepository,
            IAppUserRepository userRepository,
            ILogger<GetUserFriendsQueryHandler> logger)
        {
            _friendshipRepository = friendshipRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<AppUserDto>> Handle(GetUserFriendsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var friendships = await _friendshipRepository.GetAcceptedFriendshipsAsync(request.UserId);
                var friendIds = friendships
                    .Select(f => f.UserId == request.UserId ? f.FriendId : f.UserId)
                    .ToList();

                var friends = await _userRepository.GetUsersByIdentityIdsAsync(friendIds);
                var friendDtos = friends.Select(f => new AppUserDto
                {
                    Id = f.Id,
                    IdentityId = f.IdentityId,
                    Name = f.Name,
                    Email = f.Email,
                    Img = f.Img
                });

                _logger.LogInformation("Retrieved {Count} friends for user {UserId}", friendDtos.Count(), request.UserId);
                return friendDtos;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error retrieving friends for user {UserId}", request.UserId);
                return new List<AppUserDto>();
            }
        }
    }
} 