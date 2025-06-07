using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace SocialNetwork.Application.Features.Users.Commands.RemoveFriend
{
    public class RemoveFriendCommandHandler : IRequestHandler<RemoveFriendCommand, bool>
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IFriendHubService _friendHubService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RemoveFriendCommandHandler> _logger;

        public RemoveFriendCommandHandler(
            IFriendshipRepository friendshipRepository,
            IFriendHubService friendHubService,
            UserManager<IdentityUser> userManager,
            ILogger<RemoveFriendCommandHandler> logger)
        {
            _friendshipRepository = friendshipRepository;
            _friendHubService = friendHubService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<bool> Handle(RemoveFriendCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUser = await _userManager.FindByIdAsync(request.UserId);
                var targetUser = await _userManager.FindByIdAsync(request.FriendId);

                if (currentUser == null || targetUser == null)
                {
                    _logger.LogWarning("User not found. CurrentUserId: {CurrentUserId}, TargetUserId: {TargetUserId}",
                        request.UserId, request.FriendId);
                    return false;
                }

                var result = await _friendshipRepository.RemoveFriendAsync(request.UserId, request.FriendId);
                if (result)
                {
                    await _friendHubService.NotifyFriendRemoved(request.FriendId, currentUser);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing friend. UserId: {UserId}, FriendId: {FriendId}",
                    request.UserId, request.FriendId);
                return false;
            }
        }
    }
} 