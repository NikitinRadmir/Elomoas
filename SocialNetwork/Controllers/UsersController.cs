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
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var query = new GetAllUsersQuery();
            var allUsers = await _mediator.Send(query);
            
            // Получаем входящие заявки в друзья
            var pendingFriendships = await _friendshipRepository.GetPendingFriendshipsAsync(currentUser.Id);
            var pendingFriendIds = pendingFriendships
                .Where(f => f.FriendId == currentUser.Id) // Только входящие заявки
                .Select(f => f.UserId)
                .ToList();

            // Получаем список друзей
            var friendships = await _friendshipRepository.GetAcceptedFriendshipsAsync(currentUser.Id);
            var friendIds = friendships
                .SelectMany(f => new[] { f.UserId, f.FriendId })
                .Where(id => id != currentUser.Id)
                .ToList();

            // Фильтруем и группируем пользователей
            var pendingRequests = allUsers.Where(u => pendingFriendIds.Contains(u.IdentityId));
            var friends = allUsers.Where(u => friendIds.Contains(u.IdentityId));
            var otherUsers = allUsers.Where(u => !pendingFriendIds.Contains(u.IdentityId) && 
                                                !friendIds.Contains(u.IdentityId) && 
                                                u.IdentityId != currentUser.Id);

            // Применяем поиск, если указан
            if (!string.IsNullOrEmpty(search))
            {
                var searchLower = search.ToLower();
                pendingRequests = pendingRequests.Where(u => u.Name.ToLower().Contains(searchLower) || 
                                                           u.Email.ToLower().Contains(searchLower));
                friends = friends.Where(u => u.Name.ToLower().Contains(searchLower) || 
                                          u.Email.ToLower().Contains(searchLower));
                otherUsers = otherUsers.Where(u => u.Name.ToLower().Contains(searchLower) || 
                                                 u.Email.ToLower().Contains(searchLower));
            }

            // Объединяем результаты в нужном порядке
            var orderedUsers = pendingRequests.Concat(friends).Concat(otherUsers);
            
            var viewModel = new UserVM
            {
                Users = orderedUsers,
                PendingFriendRequests = pendingRequests,
                Friends = friends,
                SearchTerm = search
            };

            return View(viewModel);
        }

        public async Task<IActionResult> UserPage(int id)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                var query = new GetUserByIdQuery(id);
                var user = await _mediator.Send(query);

                if (user == null)
                {
                    _logger.LogWarning($"User with ID {id} not found");
                    return NotFound();
                }

                // Получаем статус дружбы
                var friendship = await _friendshipRepository.GetFriendshipAsync(currentUser.Id, user.IdentityId);
                if (friendship != null)
                {
                    user.FriendshipStatus = friendship.Status;
                    user.IsFriend = friendship.Status == Domain.Entities.Enums.FriendshipStatus.Accepted;
                    user.IsSentByMe = friendship.UserId == currentUser.Id;
                }

                var viewModel = new UserVM
                {
                    User = user
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting user with ID {id}");
                return RedirectToAction(nameof(Users));
            }
        }

        [Authorize]
        public async Task<IActionResult> MyProfile()
        {
            var identityUser = await _userManager.GetUserAsync(User);
            if (identityUser == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var appUser = await _userRepository.GetAllUsersAsync();
            var currentAppUser = appUser.FirstOrDefault(u => u.IdentityId == identityUser.Id);
            if (currentAppUser == null)
            {
                return NotFound();
            }

            var userQuery = new GetUserByIdQuery(currentAppUser.Id);
            var user = await _mediator.Send(userQuery);

            var subscribedGroupsQuery = new GetSubscribedGroupsQuery(currentAppUser.Id);
            var subscribedGroups = await _mediator.Send(subscribedGroupsQuery);

            var subscribedCoursesQuery = new GetSubscribedCoursesQuery(currentAppUser.Id);
            var subscribedCourses = await _mediator.Send(subscribedCoursesQuery);

            // Получаем список друзей
            var friends = await _friendshipService.GetUserFriendsAsync(identityUser.Id);

            var viewModel = new UserVM
            {
                User = user,
                SubscribedGroups = subscribedGroups,
                SubscribedCourses = subscribedCourses,
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
