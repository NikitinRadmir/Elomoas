using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Elomoas.Models;
using Elomoas.mvc.Models.Courses;
using Elomoas.Application.Features.Courses.Query.GetCourseById;
using Microsoft.AspNetCore.Authorization;
using Elomoas.Application.Features.Courses.Command.SubscribeToCourse;
using Elomoas.Application.Features.Courses.Command.UnsubscribeFromCourse;
using Elomoas.Application.Features.Courses.Query.CalculatePrice;
using System.Threading.Tasks;
using Elomoas.Extensions;

namespace Elomoas.Controllers
{
    public class CourseController : Controller
    {
        private readonly ILogger<CourseController> _logger;
        private readonly IMediator _mediator;

        public CourseController(ILogger<CourseController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IActionResult> Course(int id)
        {
            try
            {
                var course = await _mediator.Send(new GetCourseByIdQuery(id));
                
                if (course == null)
                    return NotFound();

                var viewModel = new CourseDetailsVM
                {
                    Course = course,
                    IsSubscribed = course.IsCurrentUserSubscribed,
                    SubscriptionInfo = course.SubscriptionInfo?.ToViewModel()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting course details for ID: {CourseId}", id);
                return RedirectToAction(nameof(Error));
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Subscribe(int courseId, int durationInMonths)
        {
            try
            {
                var command = new SubscribeToCourseCommand(courseId, durationInMonths);
                var result = await _mediator.Send(command);

                if (result)
                {
                    // Получаем информацию о подписке
                    var course = await _mediator.Send(new GetCourseByIdQuery(courseId));
                    return Json(new { 
                        success = true,
                        subscriptionPrice = course.SubscriptionInfo.SubscriptionPrice,
                        expirationDate = course.SubscriptionInfo.ExpirationDate.ToString("dd.MM.yyyy")
                    });
                }

                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error subscribing to course {CourseId}", courseId);
                return Json(new { success = false });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Unsubscribe(int courseId)
        {
            try
            {
                var command = new UnsubscribeFromCourseCommand(courseId);
                var result = await _mediator.Send(command);

                return Json(new { success = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unsubscribing from course {CourseId}", courseId);
                return Json(new { success = false });
            }
        }

        [HttpGet]
        public async Task<IActionResult> CalculatePrice(int courseId, int durationInMonths)
        {
            try
            {
                var query = new CalculatePriceQuery(courseId, durationInMonths);
                var price = await _mediator.Send(query);

                return Json(new { success = true, price = price });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating price for course {CourseId}", courseId);
                return Json(new { success = false });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
