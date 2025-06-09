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
using Elomoas.Application.Features.Friends.Queries.GetFriendship;
using Elomoas.Application.Features.Auth.Query.GetIdentityUserById;
using Elomoas.Application.Features.Auth.Query.GetCurrentIdentityUser;
using Elomoas.Application.Features.FriendHub.Commands.NotifyFriendRequestReceived;
using Elomoas.Application.Features.FriendHub.Commands.NotifyFriendRequestAccepted;
using Elomoas.Application.Features.FriendHub.Commands.NotifyFriendRequestRejected;
using SocialNetwork.Application.Features.AppUsers.Query.GetAllAllUsers;
using SocialNetwork.Application.Features.FriendHub.Commands.NotifyFriendRemoved;
using Elomoas.Application.Features.Users.Commands.HandleFriendRequest;
using Elomoas.Application.Features.Auth.Query;
using SocialNetwork.Application.Features.AppUsers.Command.DeleteUser;
using SocialNetwork.Application.Features.AppUsers.Command.UpdateUser;
using SocialNetwork.Areas.Admin.Models;

namespace Elomoas.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IMediator _mediator;


        public UsersController(
            ILogger<UsersController> logger, 
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IActionResult> Users(string search)
        {
            var currentUser = await _mediator.Send(new GetCurrentIdentityUserQuery());
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
                var currentUser = await _mediator.Send(new GetCurrentIdentityUserQuery());
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

                var friendshipQuery = new GetFriendshipQuery(currentUser.Id, user.IdentityId);
                var friendship = await _mediator.Send(friendshipQuery);
                
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HandleFriendRequest(string targetUserId, string action)
        {
            try
            {
                _logger.LogInformation("HandleFriendRequest: Starting to process request. Action: {Action}, TargetUserId: {TargetUserId}",
                    action, targetUserId);

                if (string.IsNullOrEmpty(targetUserId))
                {
                    _logger.LogWarning("HandleFriendRequest: Target user ID is empty");
                    return Json(new { success = false, message = "Target user ID is required" });
                }

                if (string.IsNullOrEmpty(action))
                {
                    _logger.LogWarning("HandleFriendRequest: Action is empty");
                    return Json(new { success = false, message = "Action is required" });
                }

                var currentUser = await _mediator.Send(new GetCurrentIdentityUserQuery());
                if (currentUser == null)
                {
                    _logger.LogWarning("HandleFriendRequest: Current user not found");
                    return Json(new { success = false, message = "Current user not found" });
                }

                _logger.LogInformation("HandleFriendRequest: Creating command. CurrentUserId: {CurrentUserId}, TargetUserId: {TargetUserId}, Action: {Action}",
                    currentUser.Id, targetUserId, action);

                var command = new HandleFriendRequestCommand(currentUser.Id, targetUserId, action);
                var result = await _mediator.Send(command);

                _logger.LogInformation("HandleFriendRequest: Command processed. Result: {Result}", result);

                if (!result)
                {
                    _logger.LogWarning("HandleFriendRequest: Command failed");
                    return Json(new { success = false, message = "Failed to process friend request" });
                }

                var message = action.ToLower() switch
                {
                    "add" or "send" => "Friend request sent successfully",
                    "accept" => "Friend request accepted",
                    "reject" => "Friend request rejected",
                    "remove" => "Friend removed",
                    _ => "Request processed successfully"
                };

                return Json(new { success = true, message = message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HandleFriendRequest: Error processing request. Action: {Action}, TargetUserId: {TargetUserId}",
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

            var currentIdentityUser = await _mediator.Send(new GetCurrentIdentityUserQuery());
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
                    await _mediator.Send(new NotifyFriendRemovedCommand 
                    { 
                        FriendId = friendId,
                        RemoverId = currentIdentityUser.Id,
                        RemoverName = currentIdentityUser.UserName
                    });
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
            var currentIdentityUser = await _mediator.Send(new GetCurrentIdentityUserQuery());
            if (currentIdentityUser == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var appUser = await _mediator.Send(new GetAllAllUsersQuery());
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
