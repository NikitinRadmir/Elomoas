using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SocialNetwork.Areas.Admin.Models;
using Elomoas.Application.Features.Friends.Commands.CreateFriendship;
using Elomoas.Application.Features.Friends.Commands.UpdateFriendship;
using Elomoas.Application.Features.Friends.Commands.DeleteFriendship;
using Elomoas.Application.Features.Friends.Queries.GetAllFriendships;
using Elomoas.Application.Features.Friends.Queries.GetFriendshipById;
using SocialNetwork.Application.Features.AppUsers.Query.GetAllAllUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Elomoas.Domain.Entities;
using Elomoas.Domain.Entities.Enums;

namespace SocialNetwork.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class FriendshipsController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<FriendshipsController> _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public FriendshipsController(
        IMediator mediator,
        ILogger<FriendshipsController> logger,
        UserManager<IdentityUser> userManager)
    {
        _mediator = mediator;
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var friendships = await _mediator.Send(new GetAllFriendshipsQuery());
        var viewModel = new FriendshipsViewModel
        {
            Friendships = friendships
        };
        return View(viewModel);
    }

    public async Task<IActionResult> Create()
    {
        var users = await _userManager.Users.ToListAsync();
        ViewBag.Users = new SelectList(users, "Id", "Email");
        return View(new CreateFriendshipViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateFriendshipViewModel model)
    {
        if (ModelState.IsValid)
        {
            var command = new CreateFriendshipCommand
            {
                UserId = model.UserId,
                FriendId = model.FriendId,
                Status = model.Status
            };

            await _mediator.Send(command);
            TempData["SuccessMessage"] = "Friendship created successfully";
            return RedirectToAction(nameof(Index));
        }

        var users = await _userManager.Users.ToListAsync();
        ViewBag.Users = new SelectList(users, "Id", "Email");
        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var friendship = await _mediator.Send(new GetFriendshipByIdQuery(id));
        if (friendship == null)
        {
            return NotFound();
        }

        var users = await _userManager.Users.ToListAsync();
        ViewBag.Users = new SelectList(users, "Id", "Email");

        var viewModel = new UpdateFriendshipViewModel
        {
            Id = friendship.Id,
            UserId = friendship.UserId,
            FriendId = friendship.FriendId,
            Status = friendship.Status
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateFriendshipViewModel model)
    {
        if (ModelState.IsValid)
        {
            var command = new UpdateFriendshipCommand
            {
                Id = model.Id,
                UserId = model.UserId,
                FriendId = model.FriendId,
                Status = model.Status
            };

            var result = await _mediator.Send(command);
            if (result)
            {
                TempData["SuccessMessage"] = "Friendship updated successfully";
                return RedirectToAction(nameof(Index));
            }
        }

        var users = await _userManager.Users.ToListAsync();
        ViewBag.Users = new SelectList(users, "Id", "Email");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteFriendshipCommand(id));
        if (result)
        {
            TempData["SuccessMessage"] = "Friendship deleted successfully";
        }
        else
        {
            TempData["ErrorMessage"] = "Error deleting friendship";
        }
        return RedirectToAction(nameof(Index));
    }
} 