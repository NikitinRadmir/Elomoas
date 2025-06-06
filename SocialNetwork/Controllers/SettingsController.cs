using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Elomoas.Models;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Elomoas.mvc.Models.Settings;
using Elomoas.Application.Features.Settings.Queries.GetAccountInfo;
using Elomoas.Application.Features.Settings.Commands.UpdateAccountInfo;
using Elomoas.Application.Features.Settings.Commands.ChangePassword;
using Elomoas.Application.Features.Auth.Query;

namespace Elomoas.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly ILogger<SettingsController> _logger;
        private readonly IMediator _mediator;

        public SettingsController(
            ILogger<SettingsController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public IActionResult Settings()
        {
            return View();
        }

        public async Task<IActionResult> AccountInfo()
        {
            var identityId = await _mediator.Send(new GetCurrentUserQuery());
            if (string.IsNullOrEmpty(identityId))
                return RedirectToAction("Login", "Auth");

            var query = new GetAccountInfoQuery(identityId);
            var accountInfo = await _mediator.Send(query);

            if (accountInfo == null)
                return RedirectToAction("Login", "Auth");

            var model = new AccountInfoVM
            {
                FirstName = accountInfo.FirstName,
                LastName = accountInfo.LastName,
                Email = accountInfo.Email,
                Description = accountInfo.Description,
                Img = accountInfo.Img
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AccountInfo(AccountInfoVM model)
        {
            var identityId = await _mediator.Send(new GetCurrentUserQuery());
            if (string.IsNullOrEmpty(identityId))
                return RedirectToAction("Login", "Auth");

            var command = new UpdateAccountInfoCommand(
                identityId,
                model.FirstName,
                model.LastName,
                model.Email,
                model.Description,
                model.ImageFile);

            var result = await _mediator.Send(command);

            if (!result)
            {
                if (!string.IsNullOrEmpty(model.Email))
                {
                    ModelState.AddModelError("Email", "This Email already exists");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to update account information");
                }
                return View(model);
            }

            TempData["SuccessMessage"] = "The data is successfully updated";
            return RedirectToAction(nameof(Settings));
        }

        public IActionResult Password()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Password(ChangePasswordVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var identityId = await _mediator.Send(new GetCurrentUserQuery());
            if (string.IsNullOrEmpty(identityId))
                return RedirectToAction("Login", "Auth");

            var command = new ChangePasswordCommand(
                identityId,
                model.CurrentPassword,
                model.NewPassword);

            var result = await _mediator.Send(command);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Failed to change password. Please check your current password.");
                return View(model);
            }

            TempData["SuccessMessage"] = "The password is successfully changed";
            return RedirectToAction(nameof(Settings));
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _mediator.Send(new Application.Features.Auth.Command.LogoutCommand());
            return RedirectToAction("Login", "Auth");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
