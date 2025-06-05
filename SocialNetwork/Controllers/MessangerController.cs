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
using Microsoft.EntityFrameworkCore;
using Elomoas.Persistence.Contexts;
using Elomoas.Domain.Entities;
using Elomoas.Application.Features.Friends.Queries.GetUserFriends;


namespace Elomoas.Controllers
{
    [Authorize]
    public class MessangerController : Controller
    {
        private readonly ILogger<MessangerController> _logger;
        private readonly IMediator _mediator;
        private readonly IFriendshipService _friendshipService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IChatService _chatService;
        private readonly ApplicationDbContext _context;

        public MessangerController(
            ILogger<MessangerController> logger, 
            IMediator mediator,
            IFriendshipService friendshipService,
            ICurrentUserService currentUserService,
            IChatService chatService,
            ApplicationDbContext context)
        {
            _logger = logger;
            _mediator = mediator;
            _friendshipService = friendshipService;
            _currentUserService = currentUserService;
            _chatService = chatService;
            _context = context;
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
                var userId = _currentUserService.IdentityId;
                
                // Получаем чаты с последними сообщениями
                var chatDtos = await (
                    from chat in _context.Chats
                    where chat.User1Id == userId || chat.User2Id == userId
                    let otherUserId = chat.User1Id == userId ? chat.User2Id : chat.User1Id
                    let lastMessage = _context.Messages
                        .Where(m => m.ChatId == chat.Id)
                        .OrderByDescending(m => m.CreatedDate)
                        .FirstOrDefault()
                    join otherUser in _context.AppUsers on otherUserId equals otherUser.IdentityId into userJoin
                    from otherUser in userJoin.DefaultIfEmpty()
                    select new
                    {
                        chatId = chat.Id,
                        userId = otherUserId,
                        userName = otherUser != null ? otherUser.Name : "Unknown",
                        userEmail = otherUser != null ? otherUser.Email : "Unknown",
                        userImage = otherUser != null ? otherUser.Img : "/images/default-icon.jpg",
                        lastMessage = lastMessage != null ? 
                            (lastMessage.Content.Length > 30 ? 
                                lastMessage.Content.Substring(0, 27) + "..." : 
                                lastMessage.Content) : 
                            "",
                        lastMessageTime = lastMessage != null ? lastMessage.CreatedDate : (DateTime?)null,
                        unreadCount = _context.Messages
                            .Count(m => m.ChatId == chat.Id && !m.IsRead && m.SenderId == otherUserId)
                    })
                    .OrderByDescending(c => c.lastMessageTime)
                    .ToListAsync();

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
                var userId = _currentUserService.IdentityId;
                var chat = await _chatService.GetOrCreateChatAsync(userId, friendId);
                var messages = await _chatService.GetChatMessagesAsync(chat.Id);
                
                // Помечаем сообщения как прочитанные
                await _chatService.MarkMessagesAsReadAsync(chat.Id, userId);

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
                var userId = _currentUserService.IdentityId;
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
