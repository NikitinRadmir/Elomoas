using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities.Enums;
using Elomoas.Hubs;
using Microsoft.Extensions.Logging;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Application.Interfaces.Services;

namespace Elomoas.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class FriendshipController : Controller
    {
        private readonly ILogger<FriendshipController> _logger;
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IAppUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly INotificationService _notificationService;
        private readonly IHubContext<FriendshipHub> _hubContext;

        public FriendshipController(
            ILogger<FriendshipController> logger,
            IFriendshipRepository friendshipRepository,
            IAppUserRepository userRepository,
            UserManager<IdentityUser> userManager,
            INotificationService notificationService,
            IHubContext<FriendshipHub> hubContext)
        {
            _logger = logger;
            _friendshipRepository = friendshipRepository;
            _userRepository = userRepository;
            _userManager = userManager;
            _notificationService = notificationService;
            _hubContext = hubContext;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendRequest(string friendId)
        {
            if (string.IsNullOrEmpty(friendId))
            {
                return Json(new { success = false, message = "Пользователь не найден" });
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            var result = await _friendshipRepository.SendFriendRequestAsync(currentUser.Id, friendId);
            if (!result)
            {
                return Json(new { success = false, message = "Не удалось отправить запрос в друзья" });
            }

            // Отправляем уведомление получателю запроса
            await _hubContext.Clients.Group(friendId).SendAsync("ReceiveFriendRequest", new
            {
                userId = currentUser.Id,
                status = FriendshipStatus.Pending.ToString(),
                isOutgoingRequest = false
            });
            
            return Json(new { 
                success = true, 
                newStatus = FriendshipStatus.Pending.ToString(),
                isOutgoingRequest = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptRequest(string friendId)
        {
            if (string.IsNullOrEmpty(friendId))
            {
                return Json(new { success = false, message = "Пользователь не найден" });
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            var result = await _friendshipRepository.AcceptFriendRequestAsync(currentUser.Id, friendId);
            if (!result)
            {
                return Json(new { success = false, message = "Не удалось принять запрос в друзья" });
            }

            // Отправляем уведомление отправителю запроса
            await _hubContext.Clients.Group(friendId).SendAsync("FriendRequestAccepted", new
            {
                userId = currentUser.Id,
                status = FriendshipStatus.Accepted.ToString(),
                isOutgoingRequest = false
            });

            return Json(new { 
                success = true, 
                newStatus = FriendshipStatus.Accepted.ToString(),
                isOutgoingRequest = false
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectRequest(string friendId)
        {
            if (string.IsNullOrEmpty(friendId))
            {
                return Json(new { success = false, message = "Пользователь не найден" });
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            var result = await _friendshipRepository.RejectFriendRequestAsync(currentUser.Id, friendId);
            if (!result)
            {
                return Json(new { success = false, message = "Не удалось отклонить запрос в друзья" });
            }

            // Отправляем уведомление отправителю запроса
            await _hubContext.Clients.Group(friendId).SendAsync("FriendRequestRejected", new
            {
                userId = currentUser.Id,
                status = "None",
                isOutgoingRequest = false
            });

            return Json(new { 
                success = true, 
                newStatus = "None",
                isOutgoingRequest = false
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFriend(string friendId)
        {
            if (string.IsNullOrEmpty(friendId))
            {
                return Json(new { success = false, message = "Пользователь не найден" });
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            var result = await _friendshipRepository.RemoveFriendAsync(currentUser.Id, friendId);
            if (!result)
            {
                return Json(new { success = false, message = "Не удалось удалить из друзей" });
            }

            // Отправляем уведомление удаленному другу
            await _hubContext.Clients.Group(friendId).SendAsync("FriendRemoved", new
            {
                userId = currentUser.Id,
                status = "None",
                isOutgoingRequest = false
            });

            return Json(new { 
                success = true, 
                newStatus = "None",
                isOutgoingRequest = false
            });
        }
    }
} 