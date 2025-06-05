using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;

namespace Elomoas.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly ICurrentUserService _currentUserService;

        public ChatHub(IChatService chatService, ICurrentUserService currentUserService)
        {
            _chatService = chatService;
            _currentUserService = currentUserService;
        }

        public async Task SendMessage(string receiverId, string message)
        {
            var senderId = _currentUserService.IdentityId;
            var chat = await _chatService.GetOrCreateChatAsync(senderId, receiverId);
            var newMessage = await _chatService.AddMessageAsync(chat.Id, senderId, message);

            var messageData = new
            {
                chatId = chat.Id,
                content = message,
                senderId = senderId,
                createdDate = newMessage.CreatedDate
            };

            // Отправляем сообщение получателю
            await Clients.User(receiverId).SendAsync("ReceiveMessage", messageData);
            
            // Отправляем подтверждение отправителю
            await Clients.User(senderId).SendAsync("MessageSent", messageData);
        }

        public async Task JoinChat(string chatId)
        {
            var userId = _currentUserService.IdentityId;
            var chat = await _chatService.GetChatByIdAsync(int.Parse(chatId));

            if (chat.User1Id == userId || chat.User2Id == userId)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
            }
        }

        public async Task LeaveChat(string chatId)
        {
            var userId = _currentUserService.IdentityId;
            var chat = await _chatService.GetChatByIdAsync(int.Parse(chatId));

            if (chat.User1Id == userId || chat.User2Id == userId)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
            }
        }

        public async Task MarkAsRead(string chatId)
        {
            var userId = _currentUserService.IdentityId;
            await _chatService.MarkMessagesAsReadAsync(int.Parse(chatId), userId);
        }

        public override async Task OnConnectedAsync()
        {
            var userId = _currentUserService.IdentityId;
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = _currentUserService.IdentityId;
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"User_{userId}");
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
} 