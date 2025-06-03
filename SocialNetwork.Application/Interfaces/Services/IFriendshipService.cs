using System.Collections.Generic;
using System.Threading.Tasks;
using Elomoas.Application.Features.AppUsers.Query;

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
    }
} 