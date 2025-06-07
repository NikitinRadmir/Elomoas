using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities.Enums;
using Elomoas.Domain.Entities;
using System;
using System.Threading.Tasks;
using Elomoas.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Elomoas.Application.Features.AppUsers.Query;

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

                _context.Friendships.Remove(request);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Friend request rejected and removed from {FriendId} by {UserId}", friendId, userId);
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

        public async Task<IEnumerable<AppUserDto>> GetUserFriendsAsync(string userId)
        {
            try
            {
                var friendships = await _context.Friendships
                    .Where(f => (f.UserId == userId || f.FriendId == userId) && 
                               f.Status == FriendshipStatus.Accepted)
                    .ToListAsync();

                var friendIds = friendships
                    .Select(f => f.UserId == userId ? f.FriendId : f.UserId)
                    .ToList();

                var friends = await _context.AppUsers
                    .Where(u => friendIds.Contains(u.IdentityId))
                    .Select(u => new AppUserDto
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Email = u.Email,
                        Description = u.Description,
                        Img = u.Img,
                        IdentityId = u.IdentityId
                    })
                    .ToListAsync();

                _logger.LogInformation("Retrieved {Count} friends for user {UserId}", friends.Count, userId);
                return friends;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving friends for user {UserId}", userId);
                return Enumerable.Empty<AppUserDto>();
            }
        }

        // Admin area CRUD operations
        public async Task<IEnumerable<Friendship>> GetAllFriendshipsAsync()
        {
            try
            {
                return await _context.Friendships.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all friendships");
                throw;
            }
        }

        public async Task<Friendship> GetFriendshipByIdAsync(int id)
        {
            try
            {
                return await _context.Friendships
                    .FirstOrDefaultAsync(f => f.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving friendship with id {Id}", id);
                throw;
            }
        }

        public async Task<Friendship> CreateFriendshipAsync(Friendship friendship)
        {
            try
            {
                _context.Friendships.Add(friendship);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Created new friendship with id {Id}", friendship.Id);
                return friendship;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating friendship");
                throw;
            }
        }

        public async Task<bool> UpdateFriendshipAsync(Friendship friendship)
        {
            try
            {
                _logger.LogInformation("Attempting to update friendship {Id}", friendship.Id);
                _context.Friendships.Update(friendship);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully updated friendship {Id}", friendship.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating friendship {Id}", friendship.Id);
                throw;
            }
        }

        public async Task<bool> DeleteFriendshipAsync(int id)
        {
            try
            {
                var friendship = await _context.Friendships.FindAsync(id);
                if (friendship == null)
                {
                    _logger.LogWarning("Friendship {Id} not found for deletion", id);
                    return false;
                }

                _context.Friendships.Remove(friendship);
                var result = await _context.SaveChangesAsync();
                
                _logger.LogInformation("Deletion affected {Count} records for friendship {Id}", result, id);
                
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting friendship {Id}", id);
                throw;
            }
        }
    }
} 