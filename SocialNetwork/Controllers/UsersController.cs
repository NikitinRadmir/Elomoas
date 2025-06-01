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

namespace Elomoas.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IMediator _mediator;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IFriendshipService _friendshipService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAppUserRepository _userRepository;

        public UsersController(
            ILogger<UsersController> logger, 
            IMediator mediator, 
            UserManager<IdentityUser> userManager, 
            IFriendshipService friendshipService,
            ICurrentUserService currentUserService,
            IAppUserRepository userRepository)
        {
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
            _friendshipService = friendshipService;
            _currentUserService = currentUserService;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Users()
        {
            var query = new GetAllUsersQuery();
            var viewModel = new UserVM
            {
                Users = await _mediator.Send(query)
            };
            return View(viewModel);
        }

        public async Task<IActionResult> UserPage(int id)
        {
            try
            {
                var query = new GetUserByIdQuery(id);
                var user = await _mediator.Send(query);

                if (user == null)
                {
                    _logger.LogWarning($"User with ID {id} not found");
                    return NotFound();
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

            var query = new GetUserByIdQuery(currentAppUser.Id);
            var viewModel = new UserVM
            {
                User = await _mediator.Send(query),
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
                return Unauthorized();
            }

            if (string.IsNullOrEmpty(targetUserId))
            {
                _logger.LogWarning("HandleFriendRequest: targetUserId is null or empty");
                TempData["Error"] = "Не указан пользователь для действия";
                return RedirectToAction(nameof(Users));
            }

            if (string.IsNullOrEmpty(action))
            {
                _logger.LogWarning("HandleFriendRequest: action is null or empty");
                TempData["Error"] = "Не указано действие";
                return RedirectToAction(nameof(Users));
            }

            _logger.LogInformation(
                "HandleFriendRequest: Attempting {Action} friendship. CurrentUser: {CurrentUserId}, TargetUser: {TargetUserId}",
                action, currentUser.Id, targetUserId);

            bool success = false;
            try
            {
                switch (action.ToLower())
                {
                    case "add":
                        success = await _friendshipService.SendFriendRequestAsync(currentUser.Id, targetUserId);
                        break;
                    case "accept":
                        success = await _friendshipService.AcceptFriendRequestAsync(currentUser.Id, targetUserId);
                        break;
                    case "reject":
                        success = await _friendshipService.RejectFriendRequestAsync(currentUser.Id, targetUserId);
                        break;
                    default:
                        _logger.LogWarning($"HandleFriendRequest: Unknown action {action}");
                        TempData["Error"] = "Неизвестное действие";
                        return RedirectToAction(nameof(Users));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HandleFriendRequest: Error processing friend request");
                success = false;
            }

            if (success)
            {
                TempData["Success"] = "Действие выполнено успешно";
            }
            else
            {
                TempData["Error"] = "Не удалось выполнить действие";
            }

            return RedirectToAction(nameof(Users));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
