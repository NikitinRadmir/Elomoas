using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Elomoas.Models;
using Elomoas.mvc.Models.Auth;
using Elomoas.Application.Features.Auth.Query;
using Elomoas.Application.Features.Auth.Command;


namespace Elomoas.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMediator _mediator;

        public AuthController(ILogger<AuthController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        [HttpGet]

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var command = new RegisterCommand
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
            };
            var isSuccess = await _mediator.Send(command);

            if (isSuccess)
                return RedirectToAction("Feed", "Home");

            ModelState.AddModelError(string.Empty, "A user with this email already exists.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var command = new LoginCommand
            {
                Email = model.Email,
                Password = model.Password,
            };

            var isSuccess = await _mediator.Send(command);
            if (isSuccess)
            {
                return RedirectToAction("Feed", "Home");
            }

            ModelState.AddModelError(string.Empty, "Wrong email or password");
            return View(model);
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
