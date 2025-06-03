using Elomoas.Domain.Entities;
using Elomoas.Domain.Entities.Enums;
using System.Threading.Tasks;

namespace Elomoas.Application.Interfaces.Repositories
{
    public interface IFriendshipRepository
    {
        Task<bool> SendFriendRequestAsync(string userId, string friendId);
        Task<bool> AcceptFriendRequestAsync(string userId, string friendId);
        Task<bool> RejectFriendRequestAsync(string userId, string friendId);
        Task<bool> RemoveFriendAsync(string userId, string friendId);
        Task<bool> AreFriendsAsync(string userId, string friendId);
        Task<FriendshipStatus?> GetFriendshipStatusAsync(string userId, string friendId);
        Task<Friendship> GetFriendshipAsync(string userId, string friendId);
    }
} 