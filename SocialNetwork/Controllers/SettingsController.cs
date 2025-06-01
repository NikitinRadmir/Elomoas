using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Elomoas.Models;
using Microsoft.AspNetCore.Identity;
using Elomoas.mvc.Models.Settings;
using Microsoft.AspNetCore.Authorization;
using Elomoas.Domain.Entities;
using Elomoas.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Elomoas.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public SettingsController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Settings()
        {
            return View();
        }

        public async Task<IActionResult> AccountInfo()
        {
            var identityUser = await _userManager.GetUserAsync(User);
            if (identityUser == null)
                return RedirectToAction("Login", "Auth");

            var appUser = await _context.AppUsers
                .FirstOrDefaultAsync(u => u.IdentityId == identityUser.Id);

            if (appUser == null)
                return RedirectToAction("Login", "Auth");

            var model = new AccountInfoVM();

            // Разбиваем имя на части
            var nameParts = appUser.Name?.Split(' ') ?? new string[0];
            model.FirstName = nameParts.Length > 0 ? nameParts[0] : "";
            model.LastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : "";

            model.Email = appUser.Email;
            model.Description = appUser.Description;
            model.Img = appUser.Img;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AccountInfo(AccountInfoVM model)
        {
            var identityUser = await _userManager.GetUserAsync(User);
            if (identityUser == null)
                return RedirectToAction("Login", "Auth");

            // Проверяем email только если он был изменен
            if (!string.IsNullOrEmpty(model.Email) && identityUser.Email != model.Email)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Этот email уже используется");
                    return View(model);
                }

                // Обновляем IdentityUser только если email изменился
                identityUser.Email = model.Email;
                identityUser.UserName = model.Email;
                var identityResult = await _userManager.UpdateAsync(identityUser);

                if (!identityResult.Succeeded)
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
            }

            // Обновляем AppUser
            var appUser = await _context.AppUsers
                .FirstOrDefaultAsync(u => u.IdentityId == identityUser.Id);

            if (appUser != null)
            {
                // Собираем полное имя только если хотя бы одно из полей заполнено
                if (!string.IsNullOrEmpty(model.FirstName) || !string.IsNullOrEmpty(model.LastName))
                {
                    var fullName = string.IsNullOrWhiteSpace(model.LastName)
                        ? model.FirstName
                        : $"{model.FirstName} {model.LastName}";
                    appUser.Name = fullName?.Trim();
                }

                if (!string.IsNullOrEmpty(model.Email))
                {
                    appUser.Email = model.Email;
                }

                if (model.Description != null) // Позволяем сохранять пустое описание
                {
                    appUser.Description = model.Description;
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Данные успешно обновлены";
            }

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

            var identityUser = await _userManager.GetUserAsync(User);
            if (identityUser == null)
                return RedirectToAction("Login", "Auth");

            // Сначала меняем пароль в IdentityUser
            var result = await _userManager.ChangePasswordAsync(identityUser, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            // Затем обновляем пароль в AppUser
            var appUser = await _context.AppUsers
                .FirstOrDefaultAsync(u => u.IdentityId == identityUser.Id);

            if (appUser != null)
            {
                appUser.Password = model.NewPassword;
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "Пароль успешно изменен";
            return RedirectToAction(nameof(Settings));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
