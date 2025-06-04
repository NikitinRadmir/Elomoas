using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Elomoas.Models;
using Microsoft.AspNetCore.Authorization;
using Elomoas.Application.Features.AppUsers.Query.GetAllUsers;
using System.Security.Claims;
using Elomoas.Application.Interfaces.Services;

namespace Elomoas.Controllers
{
    [Authorize]
    public class MessangerController : Controller
    {
        private readonly ILogger<MessangerController> _logger;
        private readonly IMediator _mediator;
        private readonly IFriendshipService _friendshipService;
        private readonly ICurrentUserService _currentUserService;

        public MessangerController(
            ILogger<MessangerController> logger, 
            IMediator mediator,
            IFriendshipService friendshipService,
            ICurrentUserService currentUserService)
        {
            _logger = logger;
            _mediator = mediator;
            _friendshipService = friendshipService;
            _currentUserService = currentUserService;
        }

        public async Task<IActionResult> Messanger()
        {
            return View();
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
                var currentUserId = _currentUserService.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Unauthorized();
                }

                var friends = await _friendshipService.GetUserFriendsAsync(currentUserId);
                return Json(friends);
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
