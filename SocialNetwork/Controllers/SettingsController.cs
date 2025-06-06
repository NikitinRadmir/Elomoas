using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Elomoas.Models;
using Microsoft.AspNetCore.Identity;
using Elomoas.mvc.Models.Settings;
using Microsoft.AspNetCore.Authorization;
using Elomoas.Domain.Entities;
using Elomoas.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Elomoas.Application.Interfaces.Services;

namespace Elomoas.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAuthService _authService;

        public SettingsController(
            UserManager<IdentityUser> userManager, 
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment,
            IAuthService authService)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _authService = authService;
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

            if (!string.IsNullOrEmpty(model.Email) && identityUser.Email != model.Email)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "This Email is already exist");
                    return View(model);
                }

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

            var appUser = await _context.AppUsers
                .FirstOrDefaultAsync(u => u.IdentityId == identityUser.Id);

            if (appUser != null)
            {
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

                if (model.Description != null) 
                {
                    appUser.Description = model.Description;
                }

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png" };
                    if (!allowedTypes.Contains(model.ImageFile.ContentType.ToLower()))
                    {
                        ModelState.AddModelError("ImageFile", "Only images in JPEG and PNG formats are allowed");
                        return View(model);
                    }

                    if (model.ImageFile.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("ImageFile", "The file size should not exceed 5MB");
                        return View(model);
                    }

                    try
                    {
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "profiles");
                        Directory.CreateDirectory(uploadsFolder);

                        var uniqueFileName = $"{Guid.NewGuid()}_{model.ImageFile.FileName}";
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        if (!string.IsNullOrEmpty(appUser.Img) && 
                            !appUser.Img.EndsWith("/images/default-icon.jpg") && 
                            !appUser.Img.EndsWith("default-icon.jpg"))
                        {
                            var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, appUser.Img.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(fileStream);
                        }

                        appUser.Img = $"/uploads/profiles/{uniqueFileName}";
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("ImageFile", "Error when loading the image. Please try again.");
                        return View(model);
                    }
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "The data is successfully updated";
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

            var result = await _userManager.ChangePasswordAsync(identityUser, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            var appUser = await _context.AppUsers
                .FirstOrDefaultAsync(u => u.IdentityId == identityUser.Id);

            if (appUser != null)
            {
                appUser.Password = model.NewPassword;
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "The password is successfully changed";
            return RedirectToAction(nameof(Settings));
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Login", "Auth");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
