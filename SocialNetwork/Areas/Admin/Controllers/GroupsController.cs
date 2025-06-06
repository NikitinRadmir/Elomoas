using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialNetwork.Areas.Admin.Models;
using Elomoas.Application.Features.Groups.Commands;
using Elomoas.Application.Features.Groups.Queries;
using SocialNetwork.Application.Features.Groups.Query.GetAllAllGroups;
using Elomoas.Domain.Entities;

namespace SocialNetwork.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class GroupsController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<GroupsController> _logger;

    public GroupsController(
        IMediator mediator,
        ILogger<GroupsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var groups = await _mediator.Send(new GetAllAllGroupsQuery());
        var viewModel = new GroupsViewModel
        {
            Groups = groups
        };
        return View(viewModel);
    }

    public IActionResult Create()
    {
        return View(new CreateGroupViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateGroupViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var command = new CreateGroupCommand
            {
                Name = model.Name,
                Description = model.Description,
                Img = model.Img,
                PL = model.PL
            };

            await _mediator.Send(command);
            TempData["SuccessMessage"] = "Group created successfully";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating group");
            TempData["ErrorMessage"] = "Failed to create group. Please try again.";
            return View(model);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        var group = await _mediator.Send(new GetGroupByIdQuery(id));
        if (group == null)
        {
            return NotFound();
        }

        var model = new UpdateGroupViewModel
        {
            Id = group.Id,
            Name = group.Name,
            Description = group.Description,
            Img = group.Img,
            PL = group.PL
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateGroupViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            _logger.LogInformation("Updating group {Id} with {@Model}", model.Id, model);

            var command = new UpdateGroupCommand
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Img = model.Img,
                PL = model.PL
            };

            var success = await _mediator.Send(command);
            
            if (success)
            {
                TempData["SuccessMessage"] = "Group updated successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "No changes were made. The group may not exist.";
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating group {Id}", model.Id);
            TempData["ErrorMessage"] = "Failed to update group. Please try again.";
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var command = new DeleteGroupCommand(id);
            var success = await _mediator.Send(command);
            
            if (success)
            {
                TempData["SuccessMessage"] = "Group deleted successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete group";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting group {Id}", id);
            TempData["ErrorMessage"] = "Failed to delete group";
        }

        return RedirectToAction(nameof(Index));
    }
} 