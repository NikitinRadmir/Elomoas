using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using Elomoas.mvc.Models.Messanger;
using Elomoas.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Elomoas.Infrastructure.Services;

public class MessageService : IMessageService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public MessageService(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<List<FriendViewModel>> GetFriendsAsync(string userId)
    {
        var friends = await _userManager.Users
            .Where(u => u.Id != userId)
            .ToListAsync();

        var friendViewModels = new List<FriendViewModel>();

        foreach (var friend in friends)
        {
            var lastMessage = await _context.Messages
                .Where(m => (m.SenderId == userId && m.ReceiverId == friend.Id) ||
                           (m.SenderId == friend.Id && m.ReceiverId == userId))
                .OrderByDescending(m => m.SentAt)
                .FirstOrDefaultAsync();

            var unreadCount = await _context.Messages
                .CountAsync(m => m.SenderId == friend.Id && m.ReceiverId == userId && !m.IsRead);

            friendViewModels.Add(new FriendViewModel
            {
                Id = friend.Id,
                Name = friend.UserName,
                ImageUrl = "/images/avatars/avatar-1.jpg",
                IsOnline = true,
                LastMessage = lastMessage?.Content,
                LastMessageTime = lastMessage?.SentAt,
                UnreadCount = unreadCount
            });
        }

        return friendViewModels;
    }

    public async Task<List<MessageViewModel>> GetChatMessagesAsync(string userId, string friendId)
    {
        var messages = await _context.Messages
            .Where(m => (m.SenderId == userId && m.ReceiverId == friendId) ||
                       (m.SenderId == friendId && m.ReceiverId == userId))
            .OrderBy(m => m.SentAt)
            .ToListAsync();

        var messageViewModels = new List<MessageViewModel>();

        foreach (var message in messages)
        {
            var sender = await _userManager.FindByIdAsync(message.SenderId);
            if (sender == null) continue;

            messageViewModels.Add(new MessageViewModel
            {
                Id = message.Id,
                SenderId = message.SenderId,
                SenderName = sender.UserName,
                SenderAvatar = "/images/avatars/avatar-1.jpg",
                Content = message.Content,
                SentAt = message.SentAt,
                IsRead = message.IsRead,
                IsMine = message.SenderId == userId
            });
        }

        return messageViewModels;
    }

    public async Task<MessageViewModel> SendMessageAsync(string senderId, string receiverId, string content)
    {
        var message = new Message
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Content = content,
            SentAt = DateTime.UtcNow,
            IsRead = false
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        var sender = await _userManager.FindByIdAsync(senderId);

        return new MessageViewModel
        {
            Id = message.Id,
            SenderId = message.SenderId,
            SenderName = sender.UserName,
            SenderAvatar = "/images/avatars/avatar-1.jpg",
            Content = message.Content,
            SentAt = message.SentAt,
            IsRead = message.IsRead,
            IsMine = true
        };
    }

    public async Task MarkMessagesAsReadAsync(string userId, string friendId)
    {
        var unreadMessages = await _context.Messages
            .Where(m => m.SenderId == friendId && m.ReceiverId == userId && !m.IsRead)
            .ToListAsync();

        foreach (var message in unreadMessages)
        {
            message.IsRead = true;
            message.ReadAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }

    public async Task<int> GetUnreadMessagesCountAsync(string userId, string friendId)
    {
        return await _context.Messages
            .CountAsync(m => m.SenderId == friendId && m.ReceiverId == userId && !m.IsRead);
    }

    public async Task<List<MessageViewModel>> GetNewMessagesAsync(string userId, string friendId, int lastMessageId)
    {
        var messages = await _context.Messages
            .Where(m => ((m.SenderId == userId && m.ReceiverId == friendId) ||
                        (m.SenderId == friendId && m.ReceiverId == userId)) &&
                        m.Id > lastMessageId)
            .OrderBy(m => m.SentAt)
            .ToListAsync();

        var messageViewModels = new List<MessageViewModel>();

        foreach (var message in messages)
        {
            var sender = await _userManager.FindByIdAsync(message.SenderId);
            if (sender == null) continue;

            messageViewModels.Add(new MessageViewModel
            {
                Id = message.Id,
                SenderId = message.SenderId,
                SenderName = sender.UserName,
                SenderAvatar = "/images/avatars/avatar-1.jpg",
                Content = message.Content,
                SentAt = message.SentAt,
                IsRead = message.IsRead,
                IsMine = message.SenderId == userId
            });
        }

        if (messages.Any())
        {
            await MarkMessagesAsReadAsync(userId, friendId);
        }

        return messageViewModels;
    }
} 