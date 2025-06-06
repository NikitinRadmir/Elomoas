using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SocialNetwork.Areas.Admin.Models;
using Elomoas.Application.Features.CourseSubscriptions.Commands;
using Elomoas.Domain.Entities;
using Elomoas.Application.Features.CourseSubscriptions.Queries;
using SocialNetwork.Application.Features.AppUsers.Query.GetAllAllUsers;
using SocialNetwork.Application.Features.Courses.Query.GetAllAllCourses;
using Microsoft.EntityFrameworkCore;

namespace SocialNetwork.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class SubscriptionsController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<SubscriptionsController> _logger;

    public SubscriptionsController(
        IMediator mediator,
        ILogger<SubscriptionsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var subscriptions = await _mediator.Send(new GetAllSubscriptionsQuery());
        var viewModel = new SubscriptionsViewModel
        {
            Subscriptions = subscriptions
        };
        return View(viewModel);
    }

    public async Task<IActionResult> Create()
    {
        var users = await _mediator.Send(new GetAllAllUsersQuery());
        var courses = await _mediator.Send(new GetAllAllCoursesQuery());

        ViewBag.Users = new SelectList(users, "Id", "Email");
        ViewBag.Courses = new SelectList(courses, "Id", "Name");

        return View(new CreateSubscriptionViewModel
        {
            ExpirationDate = DateTime.UtcNow.AddMonths(1)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateSubscriptionViewModel model)
    {
        try
        {
            // Check if subscription already exists
            var existingSubscriptions = await _mediator.Send(new GetAllSubscriptionsQuery());
            if (existingSubscriptions.Any(s => s.UserId == model.UserId && s.CourseId == model.CourseId))
            {
                TempData["ErrorMessage"] = "This user already has a subscription to this course.";
                await PrepareViewBagForCreate();
                return View(model);
            }

            // Validate expiration date
            var expirationDate = DateTime.UtcNow.AddMonths(model.DurationInMonths);
            if (model.ExpirationDate.Date != expirationDate.Date)
            {
                model.ExpirationDate = expirationDate;
            }

            var command = new CreateSubscriptionCommand
            {
                UserId = model.UserId,
                CourseId = model.CourseId,
                SubscriptionPrice = model.SubscriptionPrice,
                DurationInMonths = model.DurationInMonths,
                ExpirationDate = model.ExpirationDate
            };

            var subscription = await _mediator.Send(command);
            TempData["SuccessMessage"] = "Subscription created successfully";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("duplicate key") == true)
        {
            _logger.LogError(ex, "Duplicate subscription error for user {UserId} and course {CourseId}", model.UserId, model.CourseId);
            TempData["ErrorMessage"] = "This user already has a subscription to this course.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating subscription for user {UserId} and course {CourseId}: {Message}", 
                model.UserId, model.CourseId, ex.Message);
            TempData["ErrorMessage"] = "Failed to create subscription. Please try again.";
        }

        await PrepareViewBagForCreate();
        return View(model);
    }

    private async Task PrepareViewBagForCreate()
    {
        var users = await _mediator.Send(new GetAllAllUsersQuery());
        var courses = await _mediator.Send(new GetAllAllCoursesQuery());
        ViewBag.Users = new SelectList(users, "Id", "Email");
        ViewBag.Courses = new SelectList(courses, "Id", "Name");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var subscription = await _mediator.Send(new GetSubscriptionByIdQuery(id));
        
        if (subscription == null)
        {
            return NotFound();
        }

        var users = await _mediator.Send(new GetAllAllUsersQuery());
        var courses = await _mediator.Send(new GetAllAllCoursesQuery());
        ViewBag.Users = new SelectList(users, "Id", "Email", subscription.UserId);
        ViewBag.Courses = new SelectList(courses, "Id", "Name", subscription.CourseId);

        var model = new UpdateSubscriptionViewModel
        {
            Id = subscription.Id,
            UserId = subscription.UserId,
            CourseId = subscription.CourseId,
            SubscriptionPrice = subscription.SubscriptionPrice,
            DurationInMonths = subscription.DurationInMonths,
            ExpirationDate = subscription.ExpirationDate
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateSubscriptionViewModel model)
    {
        try
        {
            var command = new UpdateSubscriptionCommand
            {
                Id = model.Id,
                UserId = model.UserId,
                CourseId = model.CourseId,
                SubscriptionPrice = model.SubscriptionPrice,
                DurationInMonths = model.DurationInMonths,
                ExpirationDate = model.ExpirationDate
            };

            var success = await _mediator.Send(command);
            
            if (success)
            {
                TempData["SuccessMessage"] = "Subscription updated successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "No changes were made. The subscription may not exist.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating subscription {Id}", model.Id);
            TempData["ErrorMessage"] = $"Failed to update subscription: {ex.Message}";
        }

        var users = await _mediator.Send(new GetAllAllUsersQuery());
        var courses = await _mediator.Send(new GetAllAllCoursesQuery());
        ViewBag.Users = new SelectList(users, "Id", "Email", model.UserId);
        ViewBag.Courses = new SelectList(courses, "Id", "Name", model.CourseId);
        
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var command = new DeleteSubscriptionCommand(id);
            var success = await _mediator.Send(command);
            
            if (success)
            {
                TempData["SuccessMessage"] = "Subscription deleted successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete subscription";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting subscription {Id}", id);
            TempData["ErrorMessage"] = "Failed to delete subscription";
        }

        return RedirectToAction(nameof(Index));
    }
} 