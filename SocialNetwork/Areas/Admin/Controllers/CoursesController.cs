using MediatR;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Areas.Admin.Models;
using Elomoas.Domain.Entities.Enum;
using Elomoas.Application.Features.Courses.Query.GetAllCourses;
using Elomoas.Application.Features.Courses.Commands;

namespace SocialNetwork.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoursesController : Controller
    {
        private readonly IMediator _mediator;

        public CoursesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var query = new GetAllCoursesQuery();
            var courses = await _mediator.Send(query);

            var viewModel = new CoursesViewModel
            {
                Courses = courses
            };
            return View(viewModel);
        }

        public IActionResult Create()
        {
            var viewModel = new CreateCourseViewModel
            {
                AvailableLanguages = Enum.GetValues<ProgramLanguage>().ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCourseViewModel model)
        {
            var command = new CreateCourseCommand
            {
                Name = model.Name ?? "New Course",
                Description = model.Description,
                Price = model.Price,
                PL = model.PL,
                Video = model.Video ?? "/images/video4.mp4",
                Learn = model.Learn,
                Img = model.Img ?? "/images/v-1.png"
            };

            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var query = new GetAllCoursesQuery();
            var courses = await _mediator.Send(query);
            var course = courses.FirstOrDefault(c => c.Id == id);

            if (course == null)
                return NotFound();

            var viewModel = new UpdateCourseViewModel
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Price = course.Price,
                PL = course.PL,
                Video = course.Video,
                Learn = course.Learn,
                Img = course.Img,
                AvailableLanguages = Enum.GetValues<ProgramLanguage>().ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var command = new UpdateCourseCommand
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    PL = model.PL,
                    Video = model.Video,
                    Learn = model.Learn,
                    Img = model.Img
                };

                var result = await _mediator.Send(command);
                if (result)
                    return RedirectToAction(nameof(Index));
                
                ModelState.AddModelError("", "Unable to update course.");
            }

            model.AvailableLanguages = Enum.GetValues<ProgramLanguage>().ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteCourseCommand(id);
            var result = await _mediator.Send(command);
            
            if (!result)
            {
                TempData["Error"] = "Unable to delete course.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Course deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
} 