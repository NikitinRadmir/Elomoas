using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SocialNetwork.Areas.Admin.Models;
using Elomoas.Application.Features.Groups.Commands;
using Elomoas.Application.Features.Groups.Queries;

using Elomoas.Application.Features.Groups.Query;
using SocialNetwork.Application.Features.AppUsers.Query.GetAllAllUsers;
using SocialNetwork.Application.Features.Groups.Query.GetAllAllGroups;
using Elomoas.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace SocialNetwork.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "ManagerPolicy")]
public class GroupSubscriptionsController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<GroupSubscriptionsController> _logger;

    public GroupSubscriptionsController(
        IMediator mediator,
        ILogger<GroupSubscriptionsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var subscriptions = await _mediator.Send(new GetAllGroupSubscriptionsQuery());
        var viewModel = new GroupSubscriptionsViewModel
        {
            Subscriptions = subscriptions
        };
        return View(viewModel);
    }

    public async Task<IActionResult> Create()
    {
        await PrepareViewBagForCreate();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateGroupSubscriptionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PrepareViewBagForCreate();
            return View(model);
        }

        try
        {
            var command = new CreateGroupSubscriptionCommand
            {
                UserId = model.UserId,
                GroupId = model.GroupId
            };

            await _mediator.Send(command);
            TempData["SuccessMessage"] = "Group subscription created successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating subscription for user {UserId} and group {GroupId}: {Message}", 
                model.UserId, model.GroupId, ex.Message);
            TempData["ErrorMessage"] = "Failed to create subscription. Please try again.";
            await PrepareViewBagForCreate();
            return View(model);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        var subscription = await _mediator.Send(new GetGroupSubscriptionByIdQuery(id));
        
        if (subscription == null)
        {
            return NotFound();
        }

        var viewModel = new UpdateGroupSubscriptionViewModel
        {
            Id = subscription.Id,
            UserId = subscription.UserId,
            GroupId = subscription.GroupId
        };

        await PrepareViewBagForEdit(subscription);
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateGroupSubscriptionViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await PrepareViewBagForEdit(null);
            return View(model);
        }

        try
        {
            var command = new UpdateGroupSubscriptionCommand
            {
                Id = model.Id,
                UserId = model.UserId,
                GroupId = model.GroupId
            };

            var result = await _mediator.Send(command);

            if (result)
            {
                TempData["SuccessMessage"] = "Group subscription updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update subscription. Please try again.";
                await PrepareViewBagForEdit(null);
                return View(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating subscription {Id}: {Message}", id, ex.Message);
            TempData["ErrorMessage"] = "Failed to update subscription. Please try again.";
            await PrepareViewBagForEdit(null);
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _mediator.Send(new DeleteGroupSubscriptionCommand(id));
            if (result)
            {
                TempData["SuccessMessage"] = "Group subscription deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete subscription. Please try again.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting subscription {Id}: {Message}", id, ex.Message);
            TempData["ErrorMessage"] = "Failed to delete subscription. Please try again.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task PrepareViewBagForCreate()
    {
        var users = await _mediator.Send(new GetAllAllUsersQuery());
        var groups = await _mediator.Send(new GetAllAllGroupsQuery());
        ViewBag.Users = new SelectList(users, "Id", "Email");
        ViewBag.Groups = new SelectList(groups, "Id", "Name");
    }

    private async Task PrepareViewBagForEdit(GroupSubscription subscription)
    {
        var users = await _mediator.Send(new GetAllAllUsersQuery());
        var groups = await _mediator.Send(new GetAllAllGroupsQuery());
        ViewBag.Users = new SelectList(users, "Id", "Email", subscription?.UserId);
        ViewBag.Groups = new SelectList(groups, "Id", "Name", subscription?.GroupId);
    }
} 