using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace Elomoas.Application.Features.Users.Commands.HandleFriendRequest
{
    public class HandleFriendRequestCommandHandler : IRequestHandler<HandleFriendRequestCommand, bool>
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IFriendHubService _friendHubService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<HandleFriendRequestCommandHandler> _logger;

        public HandleFriendRequestCommandHandler(
            IFriendshipRepository friendshipRepository,
            IFriendHubService friendHubService,
            UserManager<IdentityUser> userManager,
            ILogger<HandleFriendRequestCommandHandler> logger)
        {
            _friendshipRepository = friendshipRepository;
            _friendHubService = friendHubService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<bool> Handle(HandleFriendRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("HandleFriendRequestCommand: Starting to process request. Action: {Action}, UserId: {UserId}, FriendId: {FriendId}",
                    request.Action, request.UserId, request.FriendId);

                // Validate request
                if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.FriendId) || string.IsNullOrEmpty(request.Action))
                {
                    _logger.LogWarning("HandleFriendRequestCommand: Invalid request parameters. UserId: {UserId}, FriendId: {FriendId}, Action: {Action}",
                        request.UserId, request.FriendId, request.Action);
                    return false;
                }

                // Validate users
                var currentUser = await _userManager.FindByIdAsync(request.UserId);
                var targetUser = await _userManager.FindByIdAsync(request.FriendId);

                if (currentUser == null || targetUser == null)
                {
                    _logger.LogWarning("HandleFriendRequestCommand: User(s) not found. CurrentUser exists: {CurrentUserExists}, TargetUser exists: {TargetUserExists}",
                        currentUser != null, targetUser != null);
                    return false;
                }

                // Prevent self-friending
                if (request.UserId == request.FriendId)
                {
                    _logger.LogWarning("HandleFriendRequestCommand: Attempted to friend self. UserId: {UserId}", request.UserId);
                    return false;
                }

                bool result = false;
                switch (request.Action.ToLower())
                {
                    case "add":
                    case "send":
                        _logger.LogInformation("HandleFriendRequestCommand: Sending friend request");
                        result = await _friendshipRepository.SendFriendRequestAsync(request.UserId, request.FriendId);
                        if (result)
                        {
                            await _friendHubService.NotifyFriendRequestReceived(request.FriendId, currentUser);
                            _logger.LogInformation("HandleFriendRequestCommand: Friend request sent successfully");
                        }
                        else
                        {
                            _logger.LogWarning("HandleFriendRequestCommand: Failed to send friend request");
                        }
                        break;

                    case "accept":
                        _logger.LogInformation("HandleFriendRequestCommand: Accepting friend request");
                        result = await _friendshipRepository.AcceptFriendRequestAsync(request.UserId, request.FriendId);
                        if (result)
                        {
                            await _friendHubService.NotifyFriendRequestAccepted(request.FriendId, currentUser);
                            _logger.LogInformation("HandleFriendRequestCommand: Friend request accepted successfully");
                        }
                        else
                        {
                            _logger.LogWarning("HandleFriendRequestCommand: Failed to accept friend request");
                        }
                        break;

                    case "reject":
                        _logger.LogInformation("HandleFriendRequestCommand: Rejecting friend request");
                        result = await _friendshipRepository.RejectFriendRequestAsync(request.UserId, request.FriendId);
                        if (result)
                        {
                            await _friendHubService.NotifyFriendRequestRejected(request.FriendId, currentUser);
                            _logger.LogInformation("HandleFriendRequestCommand: Friend request rejected successfully");
                        }
                        else
                        {
                            _logger.LogWarning("HandleFriendRequestCommand: Failed to reject friend request");
                        }
                        break;

                    case "remove":
                        _logger.LogInformation("HandleFriendRequestCommand: Removing friend");
                        result = await _friendshipRepository.RemoveFriendAsync(request.UserId, request.FriendId);
                        if (result)
                        {
                            await _friendHubService.NotifyFriendRemoved(request.FriendId, currentUser);
                            _logger.LogInformation("HandleFriendRequestCommand: Friend removed successfully");
                        }
                        else
                        {
                            _logger.LogWarning("HandleFriendRequestCommand: Failed to remove friend");
                        }
                        break;

                    default:
                        _logger.LogWarning("HandleFriendRequestCommand: Invalid action: {Action}", request.Action);
                        return false;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HandleFriendRequestCommand: Error processing request. Action: {Action}, UserId: {UserId}, FriendId: {FriendId}",
                    request.Action, request.UserId, request.FriendId);
                return false;
            }
        }
    }
} 