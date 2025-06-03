using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities.Enums;
using Elomoas.Hubs;

namespace Elomoas.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class FriendshipController : Controller
    {
        private readonly IFriendshipService _friendshipService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHubContext<FriendshipHub> _hubContext;

        public FriendshipController(
            IFriendshipService friendshipService, 
            UserManager<IdentityUser> userManager,
            IHubContext<FriendshipHub> hubContext)
        {
            _friendshipService = friendshipService;
            _userManager = userManager;
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

            var result = await _friendshipService.SendFriendRequestAsync(currentUser.Id, friendId);
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

            var result = await _friendshipService.AcceptFriendRequestAsync(currentUser.Id, friendId);
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

            var result = await _friendshipService.RejectFriendRequestAsync(currentUser.Id, friendId);
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

            var result = await _friendshipService.RemoveFriendAsync(currentUser.Id, friendId);
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