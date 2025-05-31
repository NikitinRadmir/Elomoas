using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
