using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Domain.Entities;
using Elomoas.Domain.Entities.Enums;
using System.Collections.Generic;
using System.Threading;

namespace Elomoas.Persistence.Repositories
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly IGenericRepository<Friendship> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FriendshipRepository> _logger;

        public FriendshipRepository(
            IGenericRepository<Friendship> repository,
            IUnitOfWork unitOfWork,
            ILogger<FriendshipRepository> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
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

                if (userId == friendId) return false;

                var existing = await _repository.Entities
                    .FirstOrDefaultAsync(f => (f.UserId == userId && f.FriendId == friendId) ||
                                            (f.UserId == friendId && f.FriendId == userId));

                if (existing != null)
                {
                    _logger.LogWarning("SendFriendRequestAsync: Friendship already exists with status {Status}",
                        existing.Status);
                    return false;
                }

                _logger.LogInformation("SendFriendRequestAsync: Creating new friendship");

                var friendship = new Friendship
                {
                    UserId = userId,
                    FriendId = friendId,
                    Status = FriendshipStatus.Pending,
                    AddedAt = DateTime.UtcNow
                };

                await _repository.AddAsync(friendship);
                await _unitOfWork.Save(CancellationToken.None);
                
                _logger.LogInformation("SendFriendRequestAsync: Successfully created friendship");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendFriendRequestAsync: Error sending friend request from {UserId} to {FriendId}", 
                    userId, friendId);
                return false;
            }
        }

        public async Task<bool> AcceptFriendRequestAsync(string userId, string friendId)
        {
            try
            {
                _logger.LogInformation("AcceptFriendRequestAsync: Looking for pending request from {FriendId} to {UserId}", 
                    friendId, userId);

                var request = await _repository.Entities
                    .FirstOrDefaultAsync(f => f.UserId == friendId && f.FriendId == userId && 
                                            f.Status == FriendshipStatus.Pending);

                if (request == null)
                {
                    _logger.LogWarning("AcceptFriendRequestAsync: No pending request found from {FriendId} to {UserId}", 
                        friendId, userId);
                    return false;
                }

                _logger.LogInformation("AcceptFriendRequestAsync: Accepting request from {FriendId} to {UserId}", 
                    friendId, userId);

                request.Status = FriendshipStatus.Accepted;
                await _repository.UpdateAsync(request);
                await _unitOfWork.Save(CancellationToken.None);

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

                var request = await _repository.Entities
                    .FirstOrDefaultAsync(f => f.UserId == friendId && f.FriendId == userId && 
                                            f.Status == FriendshipStatus.Pending);

                if (request == null)
                {
                    _logger.LogWarning("RejectFriendRequestAsync: No pending request found from {FriendId} to {UserId}", 
                        friendId, userId);
                    return false;
                }

                _logger.LogInformation("RejectFriendRequestAsync: Rejecting request from {FriendId} to {UserId}", 
                    friendId, userId);

                await _repository.DeleteAsync(request);
                await _unitOfWork.Save(CancellationToken.None);

                _logger.LogInformation("RejectFriendRequestAsync: Successfully rejected request from {FriendId} to {UserId}", 
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

                var friendship = await _repository.Entities
                    .FirstOrDefaultAsync(f => (f.UserId == userId && f.FriendId == friendId) ||
                                            (f.UserId == friendId && f.FriendId == userId));

                if (friendship == null)
                {
                    _logger.LogWarning("RemoveFriendAsync: No friendship found between {UserId} and {FriendId}", 
                        userId, friendId);
                    return false;
                }

                _logger.LogInformation("RemoveFriendAsync: Removing friendship between {UserId} and {FriendId}", 
                    userId, friendId);

                await _repository.DeleteAsync(friendship);
                await _unitOfWork.Save(CancellationToken.None);

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

        public async Task<bool> AreFriendsAsync(string userId, string friendId)
        {
            try
            {
                var result = await _repository.Entities
                    .AnyAsync(f => ((f.UserId == userId && f.FriendId == friendId) ||
                                   (f.UserId == friendId && f.FriendId == userId)) &&
                                 f.Status == FriendshipStatus.Accepted);

                _logger.LogInformation("AreFriendsAsync: Checked friendship status between {UserId} and {FriendId}: {Result}", 
                    userId, friendId, result);
                return result;
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
                var friendship = await GetFriendshipAsync(userId, friendId);
                var status = friendship?.Status;

                _logger.LogInformation("GetFriendshipStatusAsync: Got status {Status} for friendship between {UserId} and {FriendId}", 
                    status, userId, friendId);
                return status;
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
                var friendship = await _repository.Entities
                    .FirstOrDefaultAsync(f => (f.UserId == userId && f.FriendId == friendId) ||
                                            (f.UserId == friendId && f.FriendId == userId));

                _logger.LogInformation("GetFriendshipAsync: Retrieved friendship between {UserId} and {FriendId}: {Found}", 
                    userId, friendId, friendship != null);
                return friendship;
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
                var friendships = await _repository.Entities
                    .Where(f => (f.UserId == userId || f.FriendId == userId) && 
                               f.Status == FriendshipStatus.Pending)
                    .ToListAsync();

                _logger.LogInformation("GetPendingFriendshipsAsync: Retrieved {Count} pending friendships for user {UserId}", 
                    friendships.Count, userId);
                return friendships;
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
                var friendships = await _repository.Entities
                    .Where(f => (f.UserId == userId || f.FriendId == userId) && 
                               f.Status == FriendshipStatus.Accepted)
                    .ToListAsync();

                _logger.LogInformation("GetAcceptedFriendshipsAsync: Retrieved {Count} accepted friendships for user {UserId}", 
                    friendships.Count, userId);
                return friendships;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAcceptedFriendshipsAsync: Error getting accepted friendships for user {UserId}", 
                    userId);
                return Enumerable.Empty<Friendship>();
            }
        }
    }
} 