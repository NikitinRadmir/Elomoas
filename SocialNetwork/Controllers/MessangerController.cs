using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Elomoas.Models;
using Microsoft.AspNetCore.Authorization;
using Elomoas.Application.Features.AppUsers.Query.GetAllUsers;
using System.Security.Claims;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Application.Features.Messenger.Queries.GetUserChats;
using System.Threading.Tasks;
using System.Linq;
using Elomoas.Domain.Entities;
using Elomoas.Application.Features.Friends.Queries.GetUserFriends;
using Elomoas.Application.Features.Auth.Query.GetCurrentIdentityId;
using Elomoas.Application.Features.Messenger.Queries.GetOrCreateChat;
using Elomoas.Application.Features.Messenger.Queries.GetChatMessages;
using Elomoas.Application.Features.Messenger.Commands.MarkMessagesAsRead;


namespace Elomoas.Controllers
{
    [Authorize]
    public class MessangerController : Controller
    {
        private readonly ILogger<MessangerController> _logger;
        private readonly IMediator _mediator;


        public MessangerController(
            ILogger<MessangerController> logger, 
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;

        }

        public IActionResult Messanger()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetUserChats()
        {
            try
            {
                var userId = await _mediator.Send(new GetCurrentIdentityIdQuery());
                var chatDtos = await _mediator.Send(new GetUserChatsQuery(userId));

                return Json(chatDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user chats");
                return StatusCode(500, "Error getting user chats");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetChatMessages(string friendId)
        {
            try
            {
                var userId = await _mediator.Send(new GetCurrentIdentityIdQuery());
                var chat = await _mediator.Send(new GetOrCreateChatQuery(userId, friendId));
                var messages = await _mediator.Send(new GetChatMessagesQuery(chat.Id));
                
                // Помечаем сообщения как прочитанные
                await _mediator.Send(new MarkMessagesAsReadCommand(chat.Id, userId));

                var messageList = messages.Select(m => new
                {
                    id = m.Id,
                    chatId = m.ChatId,
                    content = m.Content,
                    senderId = m.SenderId,
                    isRead = m.IsRead,
                    createdDate = m.CreatedDate
                }).ToList();

                return Json(new 
                { 
                    messages = messageList,
                    currentUserId = userId,
                    chatId = chat.Id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting chat messages. FriendId: {FriendId}", friendId);
                return StatusCode(500, "Error getting chat messages");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetChatUsers()
        {
            try
            {
                var query = new GetAllUsersQuery();
                var users = await _mediator.Send(query);

                var chatUsers = users.Select(u => new
                {
                    id = u.Id,
                    name = u.Name,
                    img = u.Img,
                    lastMessage = "", // Это нужно будет получать из сервиса сообщений
                    unreadCount = 0,  // Это нужно будет получать из сервиса сообщений
                    isTyping = false  // Это будет обновляться через SignalR
                });

                return Json(chatUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting chat users");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFriends()
        {
            try
            {
                var userId = await _mediator.Send(new GetCurrentIdentityIdQuery());
                var fr = new GetUserFriendsQuery { UserId = userId };
                var friends = await _mediator.Send(fr);
                
                var friendDtos = friends.Select(f => new
                {
                    identityId = f.IdentityId,
                    name = f.Name,
                    email = f.Email,
                    img = f.Img ?? "/images/default-icon.jpg"
                });

                return Json(friendDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting friends list");
                return StatusCode(500, "Internal server error");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
