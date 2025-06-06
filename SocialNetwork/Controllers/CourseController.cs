using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Elomoas.Models;
using Elomoas.mvc.Models.Courses;
using Elomoas.Application.Features.Courses.Query.GetCourseById;
using Microsoft.AspNetCore.Authorization;
using Elomoas.Application.Features.Courses.Command.SubscribeToCourse;
using Elomoas.Application.Features.Courses.Command.UnsubscribeFromCourse;
using Elomoas.Application.Interfaces.Repositories;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Elomoas.Domain.Entities;
using Elomoas.Application.Features.Courses.Dto;
using Elomoas.Application.Features.Courses.Query;

namespace Elomoas.Controllers
{
    public class CourseController : Controller
    {
        private readonly ILogger<CourseController> _logger;
        private readonly IMediator _mediator;

        public CourseController(
            ILogger<CourseController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IActionResult> Course(int id)
        {
            var course = await _mediator.Send(new GetCourseByIdQuery(id));
            if (course == null)
                return NotFound();

            var viewModel = new CourseDetailsVM
            {
                Course = course,
                IsSubscribed = false
            };

            if (User.Identity.IsAuthenticated)
            {
                var identityId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (identityId != null)
                {
                    var appUserId = await _mediator.Send(new GetCurrentAppUserIdQuery(identityId));
                    if (appUserId.HasValue)
                    {
                        viewModel.IsSubscribed = await _mediator.Send(new IsSubscribedToCourseQuery(appUserId.Value, id));
                        if (viewModel.IsSubscribed)
                        {
                            viewModel.SubscriptionInfo = await _mediator.Send(new GetCourseSubscriptionInfoQuery(appUserId.Value, id));
                        }
                    }
                }
            }
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Subscribe(int courseId, int durationInMonths)
        {
            try
            {
                var identityId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (identityId == null)
                    return Json(new { success = false });
                var appUserId = await _mediator.Send(new GetCurrentAppUserIdQuery(identityId));
                if (!appUserId.HasValue)
                    return Json(new { success = false });
                var command = new SubscribeToCourseCommand(appUserId.Value, courseId, durationInMonths);
                var result = await _mediator.Send(command);
                return Json(new { success = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error subscribing to course");
                return Json(new { success = false });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Unsubscribe(int courseId)
        {
            try
            {
                var identityId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (identityId == null)
                    return Json(new { success = false });
                var appUserId = await _mediator.Send(new GetCurrentAppUserIdQuery(identityId));
                if (!appUserId.HasValue)
                    return Json(new { success = false });
                var command = new UnsubscribeFromCourseCommand(appUserId.Value, courseId);
                var result = await _mediator.Send(command);
                return Json(new { success = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unsubscribing from course");
                return Json(new { success = false });
            }
        }

        [HttpGet]
        public async Task<IActionResult> CalculatePrice(int courseId, int durationInMonths)
        {
            var course = await _mediator.Send(new GetCourseByIdQuery(courseId));
            if (course == null)
                return Json(new { success = false });
            var price = await _mediator.Send(new CalculateCoursePriceQuery(course.Price, durationInMonths));
            return Json(new { success = true, price = price });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
