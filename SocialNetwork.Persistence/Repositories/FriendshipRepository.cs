using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Domain.Entities.Enum;
using Elomoas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Elomoas.Persistence.Repositories
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly IGenericRepository<Friendship> _repository;

        public FriendshipRepository(IGenericRepository<Friendship> repository)
        {
            _repository = repository;
        }

        public async Task<bool> SendFriendRequestAsync(string userId, string friendId)
        {
            if (userId == friendId) return false;

            var existing = await _repository.Entities
                .FirstOrDefaultAsync(f => (f.UserId == userId && f.FriendId == friendId) ||
                                        (f.UserId == friendId && f.FriendId == userId));

            if (existing != null) return false;

            var friendship = new Friendship
            {
                UserId = userId,
                FriendId = friendId,
                Status = FriendshipStatus.Pending
            };

            await _repository.AddAsync(friendship);
            return true;
        }

        public async Task<bool> AcceptFriendRequestAsync(string userId, string friendId)
        {
            var request = await _repository.Entities
                .FirstOrDefaultAsync(f => f.UserId == friendId && f.FriendId == userId &&
                                        f.Status == FriendshipStatus.Pending);

            if (request == null) return false;

            request.Status = FriendshipStatus.Accepted;
            await _repository.UpdateAsync(request);
            return true;
        }

        public async Task<bool> RejectFriendRequestAsync(string userId, string friendId)
        {
            var request = await _repository.Entities
                .FirstOrDefaultAsync(f => f.UserId == friendId && f.FriendId == userId &&
                                        f.Status == FriendshipStatus.Pending);

            if (request == null) return false;

            request.Status = FriendshipStatus.Rejected;
            await _repository.UpdateAsync(request);
            return true;
        }

        public async Task<bool> RemoveFriendAsync(string userId, string friendId)
        {
            var friendship = await _repository.Entities
                .FirstOrDefaultAsync(f => (f.UserId == userId && f.FriendId == friendId) ||
                                        (f.UserId == friendId && f.FriendId == userId));

            if (friendship == null) return false;

            await _repository.DeleteAsync(friendship);
            return true;
        }

        public async Task<bool> AreFriendsAsync(string userId, string friendId)
        {
            return await _repository.Entities
                .AnyAsync(f => ((f.UserId == userId && f.FriendId == friendId) ||
                               (f.UserId == friendId && f.FriendId == userId)) &&
                             f.Status == FriendshipStatus.Accepted);
        }

        public async Task<FriendshipStatus?> GetFriendshipStatusAsync(string userId, string friendId)
        {
            var friendship = await GetFriendshipAsync(userId, friendId);
            return friendship?.Status;
        }

        public async Task<Friendship> GetFriendshipAsync(string userId, string friendId)
        {
            return await _repository.Entities
                .FirstOrDefaultAsync(f => (f.UserId == userId && f.FriendId == friendId) ||
                                        (f.UserId == friendId && f.FriendId == userId));
        }
    }
}
