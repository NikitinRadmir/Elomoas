using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Elomoas.Models;
using Elomoas.Application.Features.AppUsers.Query.GetAllUsers;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Elomoas.mvc.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Application.Features.AppUsers.Query.GetUserById;
using System.Linq;
using Elomoas.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Elomoas.Application.Features.Groups.Query.GetSubscribedGroups;
using Elomoas.Application.Features.Courses.Query.GetSubscribedCourses;
using Microsoft.AspNetCore.SignalR;
using Elomoas.Hubs;

namespace Elomoas.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IMediator _mediator;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IFriendshipService _friendshipService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAppUserRepository _userRepository;
        private readonly IHubContext<FriendshipHub> _hubContext;
        private readonly IFriendshipRepository _friendshipRepository;

        public UsersController(
            ILogger<UsersController> logger, 
            IMediator mediator, 
            UserManager<IdentityUser> userManager, 
            IFriendshipService friendshipService,
            ICurrentUserService currentUserService,
            IAppUserRepository userRepository,
            IHubContext<FriendshipHub> hubContext,
            IFriendshipRepository friendshipRepository)
        {
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
            _friendshipService = friendshipService;
            _currentUserService = currentUserService;
            _userRepository = userRepository;
            _hubContext = hubContext;
            _friendshipRepository = friendshipRepository;
        }

        public async Task<IActionResult> Users(string search)
        {
            var currentIdentityUser = await _userManager.GetUserAsync(User);
            if (currentIdentityUser == null)
                return RedirectToAction("Login", "Account");

            var allUsers = await _userRepository.GetAllUsersAsync();
            var currentUser = allUsers.FirstOrDefault(u => u.IdentityId == currentIdentityUser.Id);
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            var pendingFriendships = await _friendshipRepository.GetPendingFriendshipsAsync(currentUser.Id);
            var incomingRequests = pendingFriendships
                .Where(f => f.FriendId == currentUser.Id)
                .Select(f => allUsers.FirstOrDefault(u => u.Id == f.UserId))
                .Where(u => u != null)
                .ToList();

            var acceptedFriendships = await _friendshipRepository.GetAcceptedFriendshipsAsync(currentUser.Id);
            var friends = acceptedFriendships
                .SelectMany(f => new[] { f.UserId, f.FriendId })
                .Where(id => id != currentUser.Id)
                .Select(id => allUsers.FirstOrDefault(u => u.Id == id))
                .Where(u => u != null)
                .ToList();

            var otherUsers = allUsers
                .Where(u => u.Id != currentUser.Id)
                .Where(u => !friends.Contains(u))
                .Where(u => !incomingRequests.Contains(u))
                .ToList();

            if (!string.IsNullOrEmpty(search))
            {
                var searchLower = search.ToLower();
                incomingRequests = incomingRequests
                    .Where(u => u.FullName.ToLower().Contains(searchLower))
                    .ToList();
                friends = friends
                    .Where(u => u.FullName.ToLower().Contains(searchLower))
                    .ToList();
                otherUsers = otherUsers
                    .Where(u => u.FullName.ToLower().Contains(searchLower))
                    .ToList();
            }

            var viewModel = new UsersVM
            {
                IncomingRequests = incomingRequests,
                Friends = friends,
                OtherUsers = otherUsers,
                SearchTerm = search
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Profile(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Users");

            var currentIdentityUser = await _userManager.GetUserAsync(User);
            if (currentIdentityUser == null)
                return RedirectToAction("Login", "Account");

            var allUsers = await _userRepository.GetAllUsersAsync();
            var currentUser = allUsers.FirstOrDefault(u => u.IdentityId == currentIdentityUser.Id);
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            var targetUser = allUsers.FirstOrDefault(u => u.Id == id);
            if (targetUser == null)
                return RedirectToAction("Users");

            if (targetUser.Id == currentUser.Id)
                return RedirectToAction("MyProfile");

            var friendshipStatus = await _friendshipRepository.GetFriendshipStatusAsync(currentUser.Id, targetUser.Id);

            var viewModel = new ProfileVM
            {
                User = targetUser,
                FriendshipStatus = friendshipStatus
            };

            return View(viewModel);
        }

        public async Task<IActionResult> MyProfile()
        {
            var currentIdentityUser = await _userManager.GetUserAsync(User);
            if (currentIdentityUser == null)
                return RedirectToAction("Login", "Account");

            var allUsers = await _userRepository.GetAllUsersAsync();
            var currentUser = allUsers.FirstOrDefault(u => u.IdentityId == currentIdentityUser.Id);
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            var acceptedFriendships = await _friendshipRepository.GetAcceptedFriendshipsAsync(currentUser.Id);
            var friends = acceptedFriendships
                .SelectMany(f => new[] { f.UserId, f.FriendId })
                .Where(id => id != currentUser.Id)
                .Select(id => allUsers.FirstOrDefault(u => u.Id == id))
                .Where(u => u != null)
                .ToList();

            var viewModel = new MyProfileVM
            {
                User = currentUser,
                Friends = friends
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HandleFriendRequest(string targetUserId, string action)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                _logger.LogWarning("HandleFriendRequest: Current user not found");
                return Json(new { success = false, message = "Пользователь не авторизован" });
            }

            if (string.IsNullOrEmpty(targetUserId))
            {
                _logger.LogWarning("HandleFriendRequest: targetUserId is null or empty");
                return Json(new { success = false, message = "Не указан пользователь для действия" });
            }

            if (string.IsNullOrEmpty(action))
            {
                _logger.LogWarning("HandleFriendRequest: action is null or empty");
                return Json(new { success = false, message = "Не указано действие" });
            }

            _logger.LogInformation(
                "HandleFriendRequest: Attempting {Action} friendship. CurrentUser: {CurrentUserId}, TargetUser: {TargetUserId}",
                action, currentUser.Id, targetUserId);

            bool success = false;
            string message = "";
            try
            {
                switch (action.ToLower())
                {
                    case "add":
                        success = await _friendshipService.SendFriendRequestAsync(currentUser.Id, targetUserId);
                        if (success)
                        {
                            await _hubContext.Clients.Group(targetUserId).SendAsync("ReceiveFriendRequest", new
                            {
                                SenderId = currentUser.Id,
                                SenderName = currentUser.UserName
                            });
                        }
                        message = success ? "Запрос в друзья отправлен" : "Не удалось отправить запрос в друзья";
                        break;
                    case "accept":
                        success = await _friendshipService.AcceptFriendRequestAsync(currentUser.Id, targetUserId);
                        if (success)
                        {
                            await _hubContext.Clients.Group(targetUserId).SendAsync("FriendRequestAccepted", new
                            {
                                AcceptorId = currentUser.Id,
                                AcceptorName = currentUser.UserName
                            });
                        }
                        message = success ? "Запрос в друзья принят" : "Не удалось принять запрос в друзья";
                        break;
                    case "reject":
                        success = await _friendshipService.RejectFriendRequestAsync(currentUser.Id, targetUserId);
                        if (success)
                        {
                            await _hubContext.Clients.Group(targetUserId).SendAsync("FriendRequestRejected", new
                            {
                                RejectorId = currentUser.Id
                            });
                        }
                        message = success ? "Запрос в друзья отклонен" : "Не удалось отклонить запрос в друзья";
                        break;
                    case "remove":
                        success = await _friendshipService.RemoveFriendAsync(currentUser.Id, targetUserId);
                        if (success)
                        {
                            await _hubContext.Clients.Group(targetUserId).SendAsync("FriendRemoved", new
                            {
                                RemoverId = currentUser.Id
                            });
                        }
                        message = success ? "Пользователь удален из друзей" : "Не удалось удалить пользователя из друзей";
                        break;
                    default:
                        _logger.LogWarning($"HandleFriendRequest: Unknown action {action}");
                        return Json(new { success = false, message = "Неизвестное действие" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HandleFriendRequest: Error processing friend request");
                return Json(new { success = false, message = "Произошла ошибка при обработке запроса" });
            }

            return Json(new { success = success, message = message });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RemoveFriend(string friendId)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    return Json(new { success = false, message = "Пользователь не авторизован" });
                }

                var success = await _friendshipService.RemoveFriendAsync(currentUser.Id, friendId);
                if (success)
                {
                    await _hubContext.Clients.Group(friendId).SendAsync("FriendRemoved", new
                    {
                        RemoverId = currentUser.Id
                    });
                    return Json(new { success = true, message = "Друг успешно удален" });
                }
                else
                {
                    return Json(new { success = false, message = "Не удалось удалить друга" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении друга");
                return Json(new { success = false, message = "Произошла ошибка при удалении друга" });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
