using Elomoas.Application.Features.Auth.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Application.Features.AppUsers.Query.GetAllAllUsers;
using SocialNetwork.Areas.Admin.Models;

namespace SocialNetwork.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppUsersController : Controller
    {
        private readonly IMediator _mediator;

        public AppUsersController(IMediator mediator)
        {
            _mediator = mediator;
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
    }
} 