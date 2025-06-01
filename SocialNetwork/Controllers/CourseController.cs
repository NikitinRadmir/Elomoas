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

namespace Elomoas.Controllers
{
    public class CourseController : Controller
    {
        private readonly ILogger<CourseController> _logger;
        private readonly IMediator _mediator;
        private readonly IAppUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICourseSubscriptionRepository _subscriptionRepository;

        public CourseController(
            ILogger<CourseController> logger,
            IMediator mediator,
            IAppUserRepository userRepository,
            UserManager<IdentityUser> userManager,
            ICourseSubscriptionRepository subscriptionRepository)
        {
            _logger = logger;
            _mediator = mediator;
            _userRepository = userRepository;
            _userManager = userManager;
            _subscriptionRepository = subscriptionRepository;
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
                }
            }

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Subscribe(int courseId)
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

                var command = new SubscribeToCourseCommand(currentAppUser.Id, courseId);
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
