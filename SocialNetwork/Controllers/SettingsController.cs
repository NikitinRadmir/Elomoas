using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using MediatR;
using Elomoas.Application.Features.Settings.Queries;
using Elomoas.Application.Features.Settings.Commands;
using Elomoas.Application.Features.Settings.Dto;
using Elomoas.mvc.Models.Settings;
using Elomoas.Models;

namespace Elomoas.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly ILogger<SettingsController> _logger;
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SettingsController(ILogger<SettingsController> logger, IMediator mediator, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _mediator = mediator;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> AccountInfo()
        {
            var identityId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (identityId == null)
                return RedirectToAction("Login", "Auth");

            var dto = await _mediator.Send(new GetAccountInfoQuery(identityId));
            if (dto == null)
                return RedirectToAction("Login", "Auth");

            var model = new AccountInfoVM
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Description = dto.Description,
                Img = dto.Img
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AccountInfo(AccountInfoVM model)
        {
            var identityId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (identityId == null)
                return RedirectToAction("Login", "Auth");

            var dto = new AccountInfoDto
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Description = model.Description,
                Img = model.Img
            };

            string? base64Image = null;
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await model.ImageFile.CopyToAsync(ms);
                    base64Image = Convert.ToBase64String(ms.ToArray());
                }
            }

            var result = await _mediator.Send(new UpdateAccountInfoCommand(identityId, dto, base64Image, _webHostEnvironment.WebRootPath));
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Ошибка при обновлении данных пользователя");
                return View(model);
            }
            TempData["SuccessMessage"] = "The data is successfully updated";
            return RedirectToAction(nameof(Settings));
        }

        public IActionResult Settings()
        {
            return View();
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

            var identityId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (identityId == null)
                return RedirectToAction("Login", "Auth");

            var result = await _mediator.Send(new ChangePasswordCommand(identityId, model.CurrentPassword, model.NewPassword));
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Ошибка при смене пароля");
                return View(model);
            }
            TempData["SuccessMessage"] = "The password is successfully changed";
            return RedirectToAction(nameof(Settings));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
