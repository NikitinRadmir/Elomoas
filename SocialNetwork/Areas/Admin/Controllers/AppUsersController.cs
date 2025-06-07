using Elomoas.Application.Features.Auth.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Application.Features.AppUsers.Query.GetAllAllUsers;
using SocialNetwork.Application.Features.AppUsers.Command.DeleteUser;
using SocialNetwork.Application.Features.AppUsers.Command.UpdateUser;
using SocialNetwork.Areas.Admin.Models;

namespace SocialNetwork.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppUsersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AppUsersController> _logger;

        public AppUsersController(IMediator mediator, ILogger<AppUsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var query = new GetAllAllUsersQuery();
            var users = await _mediator.Send(query);

            var viewModel = new AppUsersViewModel
            {
                Users = users
            };
            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAppUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var command = new RegisterCommand
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                };

                await _mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var query = new GetAllAllUsersQuery();
            var users = await _mediator.Send(query);
            var user = users.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return NotFound();

            var viewModel = new UpdateAppUserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Description = user.Description,
                Img = user.Img
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateAppUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var command = new UpdateUserCommand
                {
                    Id = model.Id,
                    Name = model.Name,
                    Email = model.Email,
                    Description = model.Description,
                    Img = model.Img
                };

                var result = await _mediator.Send(command);
                if (result)
                    return RedirectToAction(nameof(Index));
                
                ModelState.AddModelError("", "Unable to update user. Email might already be in use.");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteUserCommand { Id = id };
            var result = await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }
    }
} 