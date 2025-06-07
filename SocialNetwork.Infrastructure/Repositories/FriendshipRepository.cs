using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Domain.Entities;
using Elomoas.Domain.Entities.Enums;
using Elomoas.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Infrastructure.Repositories
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FriendshipRepository> _logger;

        public FriendshipRepository(
            ApplicationDbContext context,
            ILogger<FriendshipRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AreFriendsAsync(string userId, string friendId)
        {
            try
            {
                var friendship = await _context.Friendships
                    .FirstOrDefaultAsync(f => 
                        (f.UserId == userId && f.FriendId == friendId ||
                         f.UserId == friendId && f.FriendId == userId) &&
                        f.Status == FriendshipStatus.Accepted);

                return friendship != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AreFriendsAsync: Error checking friendship status between {UserId} and {FriendId}", 
                    userId, friendId);
                return false;
            }
        }

        public async Task<FriendshipStatus?> GetFriendshipStatusAsync(string userId, string friendId)
        {
            try
            {
                var friendship = await _context.Friendships
                    .FirstOrDefaultAsync(f => 
                        f.UserId == userId && f.FriendId == friendId ||
                        f.UserId == friendId && f.FriendId == userId);

                return friendship?.Status;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetFriendshipStatusAsync: Error getting friendship status between {UserId} and {FriendId}", 
                    userId, friendId);
                return null;
            }
        }

        public async Task<Friendship> GetFriendshipAsync(string userId, string friendId)
        {
            try
            {
                return await _context.Friendships
                    .Include(f => f.User)
                    .Include(f => f.Friend)
                    .FirstOrDefaultAsync(f => 
                        f.UserId == userId && f.FriendId == friendId ||
                        f.UserId == friendId && f.FriendId == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetFriendshipAsync: Error getting friendship between {UserId} and {FriendId}", 
                    userId, friendId);
                return null;
            }
        }

        public async Task<IEnumerable<Friendship>> GetPendingFriendshipsAsync(string userId)
        {
            try
            {
                return await _context.Friendships
                    .Where(f => f.FriendId == userId && 
                               f.Status == FriendshipStatus.Pending)
                    .Include(f => f.User)
                    .Include(f => f.Friend)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPendingFriendshipsAsync: Error getting pending friendships for user {UserId}", 
                    userId);
                return Enumerable.Empty<Friendship>();
            }
        }

        public async Task<IEnumerable<Friendship>> GetAcceptedFriendshipsAsync(string userId)
        {
            try
            {
                return await _context.Friendships
                    .Where(f => (f.UserId == userId || f.FriendId == userId) && 
                               f.Status == FriendshipStatus.Accepted)
                    .Include(f => f.User)
                    .Include(f => f.Friend)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAcceptedFriendshipsAsync: Error getting accepted friendships for user {UserId}", 
                    userId);
                return Enumerable.Empty<Friendship>();
            }
        }

        public async Task<bool> SendFriendRequestAsync(string userId, string friendId)
        {
            try
            {
                _logger.LogInformation("SendFriendRequestAsync: Starting to send friend request from {UserId} to {FriendId}", 
                    userId, friendId);

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(friendId))
                {
                    _logger.LogWarning("SendFriendRequestAsync: Invalid parameters. UserId: {UserId}, FriendId: {FriendId}",
                        userId, friendId);
                    return false;
                }

                // Check if users exist
                var user = await _context.Users.FindAsync(userId);
                var friend = await _context.Users.FindAsync(friendId);

                if (user == null || friend == null)
                {
                    _logger.LogWarning("SendFriendRequestAsync: User(s) not found. User exists: {UserExists}, Friend exists: {FriendExists}",
                        user != null, friend != null);
                    return false;
                }

                // Check if friendship already exists
                var existingFriendship = await _context.Friendships
                    .FirstOrDefaultAsync(f => 
                        (f.UserId == userId && f.FriendId == friendId) ||
                        (f.UserId == friendId && f.FriendId == userId));

                if (existingFriendship != null)
                {
                    _logger.LogWarning("SendFriendRequestAsync: Friendship already exists with status {Status}",
                        existingFriendship.Status);
                    return false;
                }

                _logger.LogInformation("SendFriendRequestAsync: Creating new friendship");

                var friendship = new Friendship
                {
                    UserId = userId,
                    FriendId = friendId,
                    Status = FriendshipStatus.Pending,
                    CreatedDate = DateTime.UtcNow
                };

                try
                {
                    _context.Friendships.Add(friendship);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("SendFriendRequestAsync: Successfully created friendship");
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "SendFriendRequestAsync: Error saving friendship to database");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendFriendRequestAsync: Unexpected error");
                return false;
            }
        }

        public async Task<bool> AcceptFriendRequestAsync(string userId, string friendId)
        {
            try
            {
                _logger.LogInformation("AcceptFriendRequestAsync: Looking for pending request from {FriendId} to {UserId}", 
                    friendId, userId);

                var friendship = await _context.Friendships
                    .FirstOrDefaultAsync(f => 
                        f.UserId == friendId && 
                        f.FriendId == userId && 
                        f.Status == FriendshipStatus.Pending);

                if (friendship == null)
                {
                    _logger.LogWarning("AcceptFriendRequestAsync: No pending request found from {FriendId} to {UserId}", 
                        friendId, userId);
                    return false;
                }

                _logger.LogInformation("AcceptFriendRequestAsync: Accepting request from {FriendId} to {UserId}", 
                    friendId, userId);

                friendship.Status = FriendshipStatus.Accepted;
                friendship.UpdatedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("AcceptFriendRequestAsync: Successfully accepted request from {FriendId} to {UserId}", 
                    friendId, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AcceptFriendRequestAsync: Error accepting friend request from {FriendId} by {UserId}", 
                    friendId, userId);
                return false;
            }
        }

        public async Task<bool> RejectFriendRequestAsync(string userId, string friendId)
        {
            try
            {
                _logger.LogInformation("RejectFriendRequestAsync: Looking for pending request from {FriendId} to {UserId}", 
                    friendId, userId);

                var friendship = await _context.Friendships
                    .FirstOrDefaultAsync(f => 
                        f.UserId == friendId && 
                        f.FriendId == userId && 
                        f.Status == FriendshipStatus.Pending);

                if (friendship == null)
                {
                    _logger.LogWarning("RejectFriendRequestAsync: No pending request found from {FriendId} to {UserId}", 
                        friendId, userId);
                    return false;
                }

                _logger.LogInformation("RejectFriendRequestAsync: Removing request from {FriendId} to {UserId}", 
                    friendId, userId);

                _context.Friendships.Remove(friendship);
                await _context.SaveChangesAsync();

                _logger.LogInformation("RejectFriendRequestAsync: Successfully removed request from {FriendId} to {UserId}", 
                    friendId, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RejectFriendRequestAsync: Error rejecting friend request from {FriendId} by {UserId}", 
                    friendId, userId);
                return false;
            }
        }

        public async Task<bool> RemoveFriendAsync(string userId, string friendId)
        {
            try
            {
                _logger.LogInformation("RemoveFriendAsync: Looking for friendship between {UserId} and {FriendId}", 
                    userId, friendId);

                var friendship = await _context.Friendships
                    .FirstOrDefaultAsync(f => 
                        (f.UserId == userId && f.FriendId == friendId ||
                         f.UserId == friendId && f.FriendId == userId) &&
                        f.Status == FriendshipStatus.Accepted);

                if (friendship == null)
                {
                    _logger.LogWarning("RemoveFriendAsync: No friendship found between {UserId} and {FriendId}", 
                        userId, friendId);
                    return false;
                }

                _logger.LogInformation("RemoveFriendAsync: Removing friendship between {UserId} and {FriendId}", 
                    userId, friendId);

                _context.Friendships.Remove(friendship);
                await _context.SaveChangesAsync();

                _logger.LogInformation("RemoveFriendAsync: Successfully removed friendship between {UserId} and {FriendId}", 
                    userId, friendId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RemoveFriendAsync: Error removing friendship between {UserId} and {FriendId}", 
                    userId, friendId);
                return false;
            }
        }

        public async Task<IEnumerable<Friendship>> GetUserFriendshipsAsync(string userId)
        {
            try
            {
                return await _context.Friendships
                    .Where(f => (f.UserId == userId || f.FriendId == userId) && 
                               f.Status == FriendshipStatus.Accepted)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting friendships for user {UserId}", userId);
                return Enumerable.Empty<Friendship>();
            }
        }

        public async Task<IEnumerable<Friendship>> GetPendingFriendRequestsAsync(string userId)
        {
            try
            {
                return await _context.Friendships
                    .Where(f => f.FriendId == userId && f.Status == FriendshipStatus.Pending)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending friend requests for user {UserId}", userId);
                return Enumerable.Empty<Friendship>();
            }
        }
    }
} 