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

        public IActionResult AccountInfo()
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
