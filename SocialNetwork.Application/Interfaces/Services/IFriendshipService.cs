using System.Collections.Generic;
using System.Threading.Tasks;
using Elomoas.Application.Features.AppUsers.Query;
using Elomoas.Domain.Entities;

namespace Elomoas.Application.Interfaces.Services
{
    public interface IFriendshipService
    {
        Task<bool> SendFriendRequestAsync(string userId, string friendId);
        Task<bool> AcceptFriendRequestAsync(string userId, string friendId);
        Task<bool> RejectFriendRequestAsync(string userId, string friendId);
        Task<bool> RemoveFriendAsync(string userId, string friendId);
        Task<bool> AreFriendsAsync(string userId, string friendId);
        Task<IEnumerable<AppUserDto>> GetUserFriendsAsync(string userId);
        
        // Admin area CRUD operations
        Task<IEnumerable<Friendship>> GetAllFriendshipsAsync();
        Task<Friendship> GetFriendshipByIdAsync(int id);
        Task<Friendship> CreateFriendshipAsync(Friendship friendship);
        Task<bool> UpdateFriendshipAsync(Friendship friendship);
        Task<bool> DeleteFriendshipAsync(int id);
    }
} 