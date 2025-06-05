using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialNetwork.Areas.Admin.Models;
using Elomoas.Application.Features.Courses.Commands;
using Elomoas.Application.Features.Courses.Query.GetAllCourses;
using Elomoas.Application.Features.Courses.Query.GetCourseById;
using Elomoas.Domain.Entities.Enum;

namespace SocialNetwork.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CoursesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(
            IMediator mediator,
            ILogger<CoursesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
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
            var model = new CreateCourseViewModel
            {
                AvailableLanguages = Enum.GetValues<ProgramLanguage>().ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCourseViewModel model)
        {
            try
            {
                var command = new CreateCourseCommand
                {
                    Name = model.Name,
                    Description = model.Description,
                    Img = model.Img,
                    Price = model.Price,
                    PL = model.PL,
                    Video = model.Video,
                    Learn = model.Learn
                };

                await _mediator.Send(command);
                TempData["SuccessMessage"] = "Course created successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating course");
                ModelState.AddModelError("", "Failed to create course. Please try again.");
                model.AvailableLanguages = Enum.GetValues<ProgramLanguage>().ToList();
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var query = new GetCourseByIdQuery(id);
            var course = await _mediator.Send(query);
            
            if (course == null)
            {
                return NotFound();
            }

            var model = new UpdateCourseViewModel
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Img = course.Img,
                Price = course.Price,
                PL = course.PL,
                Video = course.Video,
                Learn = course.Learn,
                AvailableLanguages = Enum.GetValues<ProgramLanguage>().ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCourseViewModel model)
        {
            try
            {
                _logger.LogInformation("Starting course update for id {Id}", model.Id);

                var command = new UpdateCourseCommand
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Img = model.Img,
                    Price = model.Price,
                    PL = model.PL,
                    Video = model.Video,
                    Learn = model.Learn
                };

                var success = await _mediator.Send(command);
                
                if (success)
                {
                    TempData["SuccessMessage"] = "Course updated successfully";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "No changes were made. The course may not exist.");
                model.AvailableLanguages = Enum.GetValues<ProgramLanguage>().ToList();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating course {Id}", model.Id);
                ModelState.AddModelError("", "Failed to update course. Please try again.");
                model.AvailableLanguages = Enum.GetValues<ProgramLanguage>().ToList();
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var command = new DeleteCourseCommand(id);
                var success = await _mediator.Send(command);
                
                if (success)
                {
                    TempData["SuccessMessage"] = "Course deleted successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete course";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting course {Id}", id);
                TempData["ErrorMessage"] = "Failed to delete course";
            }

            return RedirectToAction(nameof(Index));
        }
    }
} 