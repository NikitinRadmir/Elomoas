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
using Elomoas.Application.Features.Friends.Commands.SendFriendRequest;
using Elomoas.Application.Features.Friends.Commands.AcceptFriendRequest;
using Elomoas.Application.Features.Friends.Commands.RejectFriendRequest;
using Elomoas.Application.Features.Friends.Commands.RemoveFriend;
using Elomoas.Application.Features.Friends.Queries.GetUserFriends;
using Elomoas.Application.Features.Friends.Queries.GetPendingFriendRequests;
using Elomoas.Infrastructure.Hubs;

namespace Elomoas.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IMediator _mediator;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAppUserRepository _userRepository;
        private readonly IHubContext<FriendshipHub> _hubContext;
        private readonly IFriendshipRepository _friendshipRepository;

        public UsersController(
            ILogger<UsersController> logger, 
            IMediator mediator, 
            UserManager<IdentityUser> userManager, 
            ICurrentUserService currentUserService,
            IAppUserRepository userRepository,
            IHubContext<FriendshipHub> hubContext,
            IFriendshipRepository friendshipRepository)
        {
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
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

            var pendingRequestsQuery = new GetPendingFriendRequestsQuery { UserId = currentUser.Id };
            var pendingFriendships = await _mediator.Send(pendingRequestsQuery);
            var pendingFriendIds = pendingFriendships
                .Where(f => f.FriendId == currentUser.Id) 
                .Select(f => f.UserId)
                .ToList();

            var friendsQuery = new GetUserFriendsQuery { UserId = currentUser.Id };
            var friends = await _mediator.Send(friendsQuery);
            var friendIds = friends.Select(f => f.IdentityId).ToList();

            var pendingRequests = allUsers.Where(u => pendingFriendIds.Contains(u.IdentityId));
            var friendUsers = allUsers.Where(u => friendIds.Contains(u.IdentityId));
            var otherUsers = allUsers.Where(u => !pendingFriendIds.Contains(u.IdentityId) && 
                                                !friendIds.Contains(u.IdentityId) && 
                                                u.IdentityId != currentUser.Id);

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                pendingRequests = pendingRequests.Where(u => 
                    u.Name.ToLower().Contains(search) || 
                    u.Email.ToLower().Contains(search));
                
                friendUsers = friendUsers.Where(u => 
                    u.Name.ToLower().Contains(search) || 
                    u.Email.ToLower().Contains(search));
                
                otherUsers = otherUsers.Where(u => 
                    u.Name.ToLower().Contains(search) || 
                    u.Email.ToLower().Contains(search));
            }
            
            var viewModel = new UserVM
            {
                SearchTerm = search,
                PendingFriendRequests = pendingRequests.ToList(),
                Friends = friendUsers.ToList(),
                Users = otherUsers.ToList()
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HandleFriendRequest(string targetUserId, string action)
        {
            if (string.IsNullOrEmpty(targetUserId))
            {
                return Json(new { success = false, message = "User ID cannot be empty" });
            }

            if (string.IsNullOrEmpty(action))
            {
                return Json(new { success = false, message = "Action cannot be empty" });
            }

            var currentIdentityUser = await _userManager.GetUserAsync(User);
            if (currentIdentityUser == null)
            {
                return Json(new { success = false, message = "User not found" });
            }

            var targetUser = await _userManager.FindByIdAsync(targetUserId);
            if (targetUser == null)
            {
                return Json(new { success = false, message = "Target user not found" });
            }

            // Prevent self-friending
            if (currentIdentityUser.Id == targetUserId)
            {
                return Json(new { success = false, message = "Cannot send friend request to yourself" });
            }

            bool result = false;
            string message = string.Empty;

            try
            {
                switch (action.ToLower().Trim())
                {
                    case "send":
                    case "add":
                        result = await _mediator.Send(new SendFriendRequestCommand(currentIdentityUser.Id, targetUserId));
                        message = result ? "Friend request sent successfully" : "Failed to send friend request";
                        if (result)
                        {
                            await _hubContext.Clients.Group(targetUserId).SendAsync("ReceiveFriendRequest", 
                                new { 
                                    senderId = currentIdentityUser.Id, 
                                    senderName = currentIdentityUser.UserName,
                                    senderEmail = currentIdentityUser.Email
                                });
                        }
                        break;

                    case "accept":
                        result = await _mediator.Send(new AcceptFriendRequestCommand(currentIdentityUser.Id, targetUserId));
                        message = result ? "Friend request accepted" : "Failed to accept friend request";
                        if (result)
                        {
                            await _hubContext.Clients.Group(targetUserId).SendAsync("FriendRequestAccepted", 
                                new { acceptorId = currentIdentityUser.Id, acceptorName = currentIdentityUser.UserName });
                        }
                        break;

                    case "reject":
                        result = await _mediator.Send(new RejectFriendRequestCommand(currentIdentityUser.Id, targetUserId));
                        message = result ? "Friend request rejected" : "Failed to reject friend request";
                        if (result)
                        {
                            await _hubContext.Clients.Group(targetUserId).SendAsync("FriendRequestRejected", 
                                new { rejectorId = currentIdentityUser.Id, rejectorName = currentIdentityUser.UserName });
                        }
                        break;

                    case "remove":
                        result = await _mediator.Send(new RemoveFriendCommand(currentIdentityUser.Id, targetUserId));
                        message = result ? "Friend removed successfully" : "Failed to remove friend";
                        if (result)
                        {
                            await _hubContext.Clients.Group(targetUserId).SendAsync("FriendRemoved", 
                                new { removerId = currentIdentityUser.Id, removerName = currentIdentityUser.UserName });
                        }
                        break;

                    default:
                        return Json(new { success = false, message = $"Invalid action: {action}. Valid actions are: add, send, accept, reject, remove" });
                }

                return Json(new { success = result, message = message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling friend request. Action: {Action}, TargetUserId: {TargetUserId}", 
                    action, targetUserId);
                return Json(new { success = false, message = "An error occurred while processing your request" });
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFriend(string friendId)
        {
            if (string.IsNullOrEmpty(friendId))
            {
                return Json(new { success = false, message = "Friend ID cannot be empty" });
            }

            var currentIdentityUser = await _userManager.GetUserAsync(User);
            if (currentIdentityUser == null)
                {
                return Json(new { success = false, message = "User not found" });
                }

            try
            {
                var result = await _mediator.Send(new RemoveFriendCommand(currentIdentityUser.Id, friendId));
                string message = result ? "Friend removed successfully" : "Failed to remove friend";
                
                if (result)
                {
                    await _hubContext.Clients.Group(friendId).SendAsync("FriendRemoved", 
                        new { removerId = currentIdentityUser.Id, removerName = currentIdentityUser.UserName });
                }

                return Json(new { success = result, message = message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing friend. FriendId: {FriendId}", friendId);
                return Json(new { success = false, message = "An error occurred while removing friend" });
            }
        }

        public async Task<IActionResult> MyProfile()
        {
            var currentIdentityUser = await _userManager.GetUserAsync(User);
            if (currentIdentityUser == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var appUser = await _userRepository.GetAllUsersAsync();
            var currentAppUser = appUser.FirstOrDefault(u => u.IdentityId == currentIdentityUser.Id);
            if (currentAppUser == null)
            {
                return NotFound();
            }

            var query = new GetUserByIdQuery(currentAppUser.Id);
            var userDto = await _mediator.Send(query);

            var subscribedGroupsQuery = new GetSubscribedGroupsQuery(currentAppUser.Id);
            var subscribedGroups = await _mediator.Send(subscribedGroupsQuery);

            var subscribedCoursesQuery = new GetSubscribedCoursesQuery(currentAppUser.Id);
            var subscribedCourses = await _mediator.Send(subscribedCoursesQuery);

            var friendsQuery = new GetUserFriendsQuery { UserId = currentIdentityUser.Id };
            var friends = await _mediator.Send(friendsQuery);

            var viewModel = new UserVM
            {
                User = userDto,
                SubscribedGroups = subscribedGroups,
                SubscribedCourses = subscribedCourses,
                Friends = friends
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
