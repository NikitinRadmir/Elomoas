using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Elomoas.Models;
using Elomoas.mvc.Models.Courses;
using Elomoas.Application.Features.Courses.Query.GetAllCourses;
using Microsoft.AspNetCore.Authorization;

namespace Elomoas.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private readonly ILogger<SearchController> _logger;
        private readonly IMediator _mediator;

        public SearchController(ILogger<SearchController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IActionResult> Search(string searchTerm = "", string category = "")
        {
            var courses = await _mediator.Send(new GetAllCoursesQuery());

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.Trim().ToLower();
                courses = courses.Where(c => 
                    c.Name.ToLower().Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(category))
            {
                courses = courses.Where(c => c.PL.ToString() == category);
            }

            var viewModel = new CourseVM
            {
                PopularCourses = courses,
                SearchTerm = searchTerm,
                SelectedCategory = category
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
