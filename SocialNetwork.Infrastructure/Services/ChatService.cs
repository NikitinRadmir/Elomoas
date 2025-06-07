using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using Elomoas.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elomoas.Application.Features.Messenger.Queries.GetUserChats;

namespace Elomoas.Infrastructure.Services
{
    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ChatService> _logger;

        public ChatService(ApplicationDbContext context, ILogger<ChatService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Chat>> GetAllChatsAsync()
        {
            try
            {
                return await _context.Chats
                    .Include(c => c.Messages)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all chats");
                throw;
            }
        }

        public async Task<Chat> GetChatByIdAsync(int chatId)
        {
            try
            {
                var chat = await _context.Chats
                    .Include(c => c.Messages)
                    .FirstOrDefaultAsync(c => c.Id == chatId);

                if (chat == null)
                    throw new InvalidOperationException($"Chat with ID {chatId} not found");

                return chat;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting chat by ID {ChatId}", chatId);
                throw;
            }
        }

        public async Task<Chat> CreateChatAsync(Chat chat)
        {
            try
            {
                _context.Chats.Add(chat);
                await _context.SaveChangesAsync();
                return chat;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating chat");
                throw;
            }
        }

        public async Task<Chat> UpdateChatAsync(Chat chat)
        {
            try
            {
                var existingChat = await _context.Chats.FindAsync(chat.Id);
                if (existingChat == null)
                {
                    throw new KeyNotFoundException($"Chat with ID {chat.Id} not found");
                }

                existingChat.User1Id = chat.User1Id;
                existingChat.User2Id = chat.User2Id;

                await _context.SaveChangesAsync();
                return existingChat;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating chat with id {Id}", chat.Id);
                throw;
            }
        }

        public async Task DeleteChatAsync(int id)
        {
            try
            {
                var chat = await _context.Chats.FindAsync(id);
                if (chat == null)
                {
                    throw new KeyNotFoundException($"Chat with ID {id} not found");
                }

                _context.Chats.Remove(chat);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting chat with id {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Message>> GetChatMessagesAsync(int chatId)
        {
            try
            {
                return await _context.Messages
                    .Where(m => m.ChatId == chatId)
                    .OrderBy(m => m.CreatedDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting messages for chat {ChatId}", chatId);
                throw;
            }
        }

        public async Task<Message> SendMessageAsync(string senderId, string recipientId, string content)
        {
            try
            {
                var chat = await GetOrCreateChatAsync(senderId, recipientId);

                var message = new Message
                {
                    ChatId = chat.Id,
                    SenderId = senderId,
                    Content = content,
                    IsRead = false,
                    CreatedDate = DateTime.UtcNow
                };

                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();

                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message from {SenderId} to {RecipientId}", senderId, recipientId);
                throw;
            }
        }

        public async Task<Chat> GetOrCreateChatAsync(string user1Id, string user2Id)
        {
            try
            {
                var chat = await _context.Chats
                    .Include(c => c.Messages)
                    .FirstOrDefaultAsync(c =>
                        (c.User1Id == user1Id && c.User2Id == user2Id) ||
                        (c.User1Id == user2Id && c.User2Id == user1Id));

                if (chat == null)
                {
                    chat = new Chat
                    {
                        User1Id = user1Id,
                        User2Id = user2Id,
                        Messages = new List<Message>()
                    };

                    await _context.Chats.AddAsync(chat);
                    await _context.SaveChangesAsync();
                }

                return chat;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting or creating chat for users {User1Id} and {User2Id}", user1Id, user2Id);
                throw;
            }
        }

        public async Task<IEnumerable<Chat>> GetUserChatsAsync(string userId)
        {
            try
            {
                // Получаем ID последних сообщений для каждого чата
                var lastMessageIds = await _context.Messages
                    .GroupBy(m => m.ChatId)
                    .Select(g => new
                    {
                        ChatId = g.Key,
                        LastMessageId = g.OrderByDescending(m => m.CreatedDate)
                            .Select(m => m.Id)
                            .FirstOrDefault()
                    })
                    .ToListAsync();

                // Загружаем чаты с последними сообщениями
                var chats = await _context.Chats
                    .Where(c => c.User1Id == userId || c.User2Id == userId)
                    .Select(c => new Chat
                    {
                        Id = c.Id,
                        User1Id = c.User1Id,
                        User2Id = c.User2Id,
                        Messages = _context.Messages
                            .Where(m => lastMessageIds
                                .Where(lm => lm.ChatId == c.Id)
                                .Select(lm => lm.LastMessageId)
                                .Contains(m.Id))
                            .ToList()
                    })
                    .ToListAsync();

                // Сортируем чаты по времени последнего сообщения
                return chats.OrderByDescending(c => 
                    c.Messages.FirstOrDefault()?.CreatedDate ?? DateTime.MinValue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting chats for user {UserId}", userId);
                throw;
            }
        }

        public async Task MarkMessagesAsReadAsync(int chatId, string userId)
        {
            try
            {
                var unreadMessages = await _context.Messages
                    .Where(m => m.ChatId == chatId && m.SenderId != userId && !m.IsRead)
                    .ToListAsync();

                if (unreadMessages.Any())
                {
                    foreach (var message in unreadMessages)
                    {
                        message.IsRead = true;
                        message.UpdatedDate = DateTime.UtcNow;
                    }

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking messages as read in chat {ChatId} for user {UserId}", chatId, userId);
                throw;
            }
        }

        public async Task<int> GetUnreadMessagesCountAsync(string userId)
        {
            try
            {
                return await _context.Messages
                    .CountAsync(m => 
                        (m.Chat.User1Id == userId || m.Chat.User2Id == userId) && 
                        m.SenderId != userId && 
                        !m.IsRead);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting unread messages count for user {UserId}", userId);
                throw;
            }
        }

        public async Task<Message> AddMessageAsync(int chatId, string senderId, string content)
        {
            try
            {
                var chat = await GetChatByIdAsync(chatId);

                var message = new Message
                {
                    ChatId = chatId,
                    SenderId = senderId,
                    Content = content,
                    IsRead = false,
                    CreatedDate = DateTime.UtcNow
                };

                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();

                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding message to chat {ChatId} from user {SenderId}", chatId, senderId);
                throw;
            }
        }

        public async Task<Message> GetMessageByIdAsync(int messageId)
        {
            try
            {
                var message = await _context.Messages
                    .Include(m => m.Chat)
                    .FirstOrDefaultAsync(m => m.Id == messageId);

                if (message == null)
                {
                    throw new KeyNotFoundException($"Message with ID {messageId} not found");
                }

                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting message by id {MessageId}", messageId);
                throw;
            }
        }

        public async Task<Message> UpdateMessageAsync(Message message)
        {
            try
            {
                var existingMessage = await _context.Messages.FindAsync(message.Id);
                if (existingMessage == null)
                {
                    throw new KeyNotFoundException($"Message with ID {message.Id} not found");
                }

                existingMessage.Content = message.Content;
                existingMessage.IsRead = message.IsRead;

                await _context.SaveChangesAsync();
                return existingMessage;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating message with id {MessageId}", message.Id);
                throw;
            }
        }

        public async Task DeleteMessageAsync(int messageId)
        {
            try
            {
                var message = await _context.Messages.FindAsync(messageId);
                if (message == null)
                {
                    throw new KeyNotFoundException($"Message with ID {messageId} not found");
                }

                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting message with id {MessageId}", messageId);
                throw;
            }
        }

        public async Task<IEnumerable<UserChatDto>> GetUserChatsWithDetailsAsync(string userId)
        {
            try
            {
                return await (
                    from chat in _context.Chats
                    where chat.User1Id == userId || chat.User2Id == userId
                    let otherUserId = chat.User1Id == userId ? chat.User2Id : chat.User1Id
                    let lastMessage = _context.Messages
                        .Where(m => m.ChatId == chat.Id)
                        .OrderByDescending(m => m.CreatedDate)
                        .FirstOrDefault()
                    join otherUser in _context.AppUsers on otherUserId equals otherUser.IdentityId into userJoin
                    from otherUser in userJoin.DefaultIfEmpty()
                    select new UserChatDto
                    {
                        ChatId = chat.Id,
                        UserId = otherUserId,
                        UserName = otherUser != null ? otherUser.Name : "Unknown",
                        UserEmail = otherUser != null ? otherUser.Email : "Unknown",
                        UserImage = otherUser != null ? otherUser.Img : "/images/default-icon.jpg",
                        LastMessage = lastMessage != null ? 
                            (lastMessage.Content.Length > 30 ? 
                                lastMessage.Content.Substring(0, 27) + "..." : 
                                lastMessage.Content) : 
                            "",
                        LastMessageTime = lastMessage != null ? lastMessage.CreatedDate : null,
                        UnreadCount = _context.Messages
                            .Count(m => m.ChatId == chat.Id && !m.IsRead && m.SenderId == otherUserId)
                    })
                    .OrderByDescending(c => c.LastMessageTime)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting chat details for user {UserId}", userId);
                throw;
            }
        }
    }
} 