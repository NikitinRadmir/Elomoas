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

namespace Elomoas.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IMediator _mediator;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IFriendshipService _friendshipService;

        public UsersController(
            ILogger<UsersController> logger, 
            IMediator mediator, 
            UserManager<IdentityUser> userManager, 
            IFriendshipService friendshipService)
        {
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
            _friendshipService = friendshipService;
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
        public async Task<IActionResult> UserPage()
        {
            return View();
        }

        public async Task<IActionResult> MyProfile()
        {
            return View();
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
                    case "send":
                        success = await _friendshipService.SendFriendRequestAsync(currentUser.Id, targetUserId);
                        break;
                    case "accept":
                        success = await _friendshipService.AcceptFriendRequestAsync(currentUser.Id, targetUserId);
                        break;
                    case "reject":
                        success = await _friendshipService.RejectFriendRequestAsync(currentUser.Id, targetUserId);
                        break;
                    case "remove":
                        success = await _friendshipService.RemoveFriendAsync(currentUser.Id, targetUserId);
                        break;
                    default:
                        _logger.LogWarning("HandleFriendRequest: Unknown action: {Action}", action);
                        TempData["Error"] = "Неизвестное действие";
                        return RedirectToAction(nameof(Users));
                }

                if (!success)
                {
                    _logger.LogWarning(
                        "HandleFriendRequest: Action {Action} failed. CurrentUser: {CurrentUserId}, TargetUser: {TargetUserId}",
                        action, currentUser.Id, targetUserId);
                    //TempData["Error"] = GetErrorMessage(action);
                }
                else
                {
                    _logger.LogInformation(
                        "HandleFriendRequest: Action {Action} succeeded. CurrentUser: {CurrentUserId}, TargetUser: {TargetUserId}",
                        action, currentUser.Id, targetUserId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "HandleFriendRequest: Exception during {Action}. CurrentUser: {CurrentUserId}, TargetUser: {TargetUserId}",
                    action, currentUser.Id, targetUserId);
                TempData["Error"] = "Произошла ошибка при выполнении действия";
            }

            return RedirectToAction(nameof(Users));
        }

        //private void GetErrorMessage(string action) => action.ToLower() switch
        //{
        //    //"send" => "Не удалось отправить заявку в друзья. Возможно, заявка уже существует.",
        //    //"accept" => "Не удалось принять заявку в друзья. Возможно, заявка была отменена.",
        //    //"reject" => "Не удалось отклонить заявку в друзья.",
        //    //"remove" => "Не удалось удалить из друзей.",
        //    //_ => "Не удалось выполнить действие. Пожалуйста, попробуйте снова."

        //};


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
