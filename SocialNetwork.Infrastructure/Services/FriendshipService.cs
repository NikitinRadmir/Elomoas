using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using Elomoas.Domain.Entities.Enum;
using Elomoas.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Elomoas.Infrastructure.Services
{
    public class FriendshipService : IFriendshipService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<FriendshipService> _logger;

        public FriendshipService(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            ILogger<FriendshipService> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<bool> SendFriendRequestAsync(string userId, string friendId)
        {
            try
            {
                if (userId == friendId)
                {
                    _logger.LogWarning("Attempted to send friend request to self. UserId: {UserId}", userId);
                    return false;
                }

                var existing = await _context.Friendships
                    .Where(f => (f.UserId == userId && f.FriendId == friendId) ||
                                (f.UserId == friendId && f.FriendId == userId))
                    .FirstOrDefaultAsync();

                if (existing != null)
                {
                    _logger.LogInformation("Friend request already exists between users {UserId} and {FriendId}. Status: {Status}",
                        userId, friendId, existing.Status);
                    return false;
                }

                var friendship = new Friendship
                {
                    UserId = userId,
                    FriendId = friendId,
                    Status = FriendshipStatus.Pending
                };

                await _context.Friendships.AddAsync(friendship);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Friend request sent from {UserId} to {FriendId}", userId, friendId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending friend request from {UserId} to {FriendId}", userId, friendId);
                return false;
            }
        }

        public async Task<bool> AcceptFriendRequestAsync(string userId, string friendId)
        {
            try
            {
                var request = await _context.Friendships
                    .Where(f => f.UserId == friendId && f.FriendId == userId && f.Status == FriendshipStatus.Pending)
                    .FirstOrDefaultAsync();

                if (request == null)
                {
                    _logger.LogWarning("No pending friend request found from {FriendId} to {UserId}", friendId, userId);
                    return false;
                }

                request.Status = FriendshipStatus.Accepted;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Friend request accepted from {FriendId} by {UserId}", friendId, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error accepting friend request from {FriendId} by {UserId}", friendId, userId);
                return false;
            }
        }

        public async Task<bool> RejectFriendRequestAsync(string userId, string friendId)
        {
            try
            {
                var request = await _context.Friendships
                    .Where(f => f.UserId == friendId && f.FriendId == userId && f.Status == FriendshipStatus.Pending)
                    .FirstOrDefaultAsync();

                if (request == null)
                {
                    _logger.LogWarning("No pending friend request found from {FriendId} to {UserId}", friendId, userId);
                    return false;
                }

                request.Status = FriendshipStatus.Rejected;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Friend request rejected from {FriendId} by {UserId}", friendId, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting friend request from {FriendId} by {UserId}", friendId, userId);
                return false;
            }
        }

        public async Task<bool> RemoveFriendAsync(string userId, string friendId)
        {
            try
            {
                var friendship = await _context.Friendships
                    .Where(f => (f.UserId == userId && f.FriendId == friendId) ||
                                (f.UserId == friendId && f.FriendId == userId))
                    .FirstOrDefaultAsync();

                if (friendship == null)
                {
                    _logger.LogWarning("No friendship found between {UserId} and {FriendId}", userId, friendId);
                    return false;
                }

                _context.Friendships.Remove(friendship);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Friendship removed between {UserId} and {FriendId}", userId, friendId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing friendship between {UserId} and {FriendId}", userId, friendId);
                return false;
            }
        }

        public async Task<bool> AreFriendsAsync(string userId, string friendId)
        {
            try
            {
                var areFriends = await _context.Friendships
                    .AnyAsync(f => ((f.UserId == userId && f.FriendId == friendId) ||
                                    (f.UserId == friendId && f.FriendId == userId)) &&
                                f.Status == FriendshipStatus.Accepted);

                _logger.LogDebug("Friendship status checked between {UserId} and {FriendId}. Are friends: {AreFriends}",
                    userId, friendId, areFriends);

                return areFriends;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking friendship status between {UserId} and {FriendId}", userId, friendId);
                return false;
            }
        }
    }
}
