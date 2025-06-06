using Elomoas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elomoas.Application.Interfaces.Services
{
    public interface IChatService
    {
        Task<Chat> GetOrCreateChatAsync(string user1Id, string user2Id);
        Task<IEnumerable<Chat>> GetUserChatsAsync(string userId);
        Task<Message> SendMessageAsync(string senderId, string recipientId, string content);
        Task<IEnumerable<Message>> GetChatMessagesAsync(int chatId);
        Task MarkMessagesAsReadAsync(int chatId, string userId);
        Task<int> GetUnreadMessagesCountAsync(string userId);
        Task<Chat> GetChatByIdAsync(int chatId);
        Task<Message> AddMessageAsync(int chatId, string senderId, string content);
        Task<IEnumerable<Chat>> GetAllChatsAsync();
        Task<Chat> CreateChatAsync(Chat chat);
        Task<Chat> UpdateChatAsync(Chat chat);
        Task DeleteChatAsync(int id);
    }
}