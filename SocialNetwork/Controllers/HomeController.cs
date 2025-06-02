using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Elomoas.Models;
using Elomoas.mvc.Models.Courses;
using Elomoas.Application.Features.Courses.Query.GetAllCourses;
using Elomoas.Application.Features.Courses.Query.GetLatestCourses;
using Elomoas.Application.Features.Courses.Query.GetCoursesCount;
using Microsoft.AspNetCore.Authorization;

namespace Elomoas.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMediator _mediator;

    public HomeController(ILogger<HomeController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<IActionResult> Feed()
    {
        var viewModel = new CourseVM
        {
            PopularCourses = await _mediator.Send(new GetAllCoursesQuery()),
            LatestCourses = await _mediator.Send(new GetLatestCoursesQuery()),
            CoursesCount = await _mediator.Send(new GetCoursesCountQuery())
        };

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
