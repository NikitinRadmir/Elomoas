using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Elomoas.Models;
using Elomoas.mvc.Models.Courses;
using Elomoas.Application.Features.Courses.Query.GetCourseById;

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
            var course = await _mediator.Send(new GetCourseByIdQuery(id));
            
            if (course == null)
                return NotFound();

            var viewModel = new CourseDetailsVM
            {
                Course = course
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
