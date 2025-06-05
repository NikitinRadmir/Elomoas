using Microsoft.AspNetCore.Mvc;
using MediatR;
using SocialNetwork.Areas.Admin.Models;
using SocialNetwork.Application.Features.AppUsers.Query.GetAllAllUsers;
using Elomoas.Controllers;

namespace SocialNetwork.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppUsersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        public AppUsersController(IMediator mediator, ILogger<AuthController> logger)
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
    }
} 