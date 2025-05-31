/*using Elomoas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elomoas.Application.Interfaces.Services
{
    public interface IMessageService
    {
        Task<List<FriendViewModel>> GetUserFriendsWithLastMessageAsync(string userId);
        Task<List<MessageViewModel>> GetChatMessagesAsync(string userId, string friendId);
        Task<MessageViewModel> SendMessageAsync(string senderId, string receiverId, string content);
        Task MarkMessagesAsReadAsync(string userId, string friendId);
        Task<int> GetUnreadMessagesCountAsync(string userId, string friendId);
        Task<List<MessageViewModel>> GetNewMessagesAsync(string userId, string friendId, int lastMessageId);
    }
}
*/