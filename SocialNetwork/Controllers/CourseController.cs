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

namespace Elomoas.Controllers
{
    public class CourseController : Controller
    {
        private readonly ILogger<CourseController> _logger;
        private readonly IMediator _mediator;
        private readonly IAppUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICourseSubscriptionRepository _subscriptionRepository;
        private readonly IGenericRepository<CourseSubscription> _subscriptionGenericRepository;

        public CourseController(
            ILogger<CourseController> logger,
            IMediator mediator,
            IAppUserRepository userRepository,
            UserManager<IdentityUser> userManager,
            ICourseSubscriptionRepository subscriptionRepository,
            IGenericRepository<CourseSubscription> subscriptionGenericRepository)
        {
            _logger = logger;
            _mediator = mediator;
            _userRepository = userRepository;
            _userManager = userManager;
            _subscriptionRepository = subscriptionRepository;
            _subscriptionGenericRepository = subscriptionGenericRepository;
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
                var identityUser = await _userManager.GetUserAsync(User);
                var appUser = await _userRepository.GetAllUsersAsync();
                var currentAppUser = appUser.FirstOrDefault(u => u.IdentityId == identityUser.Id);
                
                if (currentAppUser != null)
                {
                    viewModel.IsSubscribed = await _subscriptionRepository.IsSubscribed(currentAppUser.Id, id);

                    if (viewModel.IsSubscribed)
                    {
                        var subscription = await _subscriptionGenericRepository.Entities
                            .FirstOrDefaultAsync(s => s.UserId == currentAppUser.Id && s.CourseId == id);

                        if (subscription != null)
                        {
                            viewModel.SubscriptionInfo = new SubscriptionInfo
                            {
                                DurationInMonths = subscription.DurationInMonths,
                                SubscriptionPrice = subscription.SubscriptionPrice,
                                ExpirationDate = subscription.ExpirationDate
                            };
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
                var identityUser = await _userManager.GetUserAsync(User);
                if (identityUser == null)
                {
                    return Json(new { success = false });
                }

                var appUser = await _userRepository.GetAllUsersAsync();
                var currentAppUser = appUser.FirstOrDefault(u => u.IdentityId == identityUser.Id);
                if (currentAppUser == null)
                {
                    return Json(new { success = false });
                }

                var command = new SubscribeToCourseCommand(currentAppUser.Id, courseId, durationInMonths);
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
                var identityUser = await _userManager.GetUserAsync(User);
                if (identityUser == null)
                {
                    return Json(new { success = false });
                }

                var appUser = await _userRepository.GetAllUsersAsync();
                var currentAppUser = appUser.FirstOrDefault(u => u.IdentityId == identityUser.Id);
                if (currentAppUser == null)
                {
                    return Json(new { success = false });
                }

                var command = new UnsubscribeFromCourseCommand(currentAppUser.Id, courseId);
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

            var option = new SubscriptionDurationOption { Months = durationInMonths };
            switch (durationInMonths)
            {
                case 3:
                    option.DiscountPercent = 10;
                    break;
                case 6:
                    option.DiscountPercent = 20;
                    break;
                case 12:
                    option.DiscountPercent = 30;
                    break;
                default:
                    option.DiscountPercent = 0;
                    break;
            }

            var price = option.CalculatePrice(course.Price);
            return Json(new { success = true, price = price });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
