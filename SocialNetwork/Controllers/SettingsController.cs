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

namespace Elomoas.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SettingsController(
            UserManager<IdentityUser> userManager, 
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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

                // Обработка загруженного изображения
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    // Проверяем тип файла
                    var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png" };
                    if (!allowedTypes.Contains(model.ImageFile.ContentType.ToLower()))
                    {
                        ModelState.AddModelError("ImageFile", "Разрешены только изображения в форматах JPEG и PNG");
                        return View(model);
                    }

                    // Проверяем размер файла (например, максимум 5MB)
                    if (model.ImageFile.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("ImageFile", "Размер файла не должен превышать 5MB");
                        return View(model);
                    }

                    try
                    {
                        // Создаем путь для сохранения
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "profiles");
                        Directory.CreateDirectory(uploadsFolder); // Создаем папку, если её нет

                        // Генерируем уникальное имя файла
                        var uniqueFileName = $"{Guid.NewGuid()}_{model.ImageFile.FileName}";
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Удаляем старый файл, если он существует
                        if (!string.IsNullOrEmpty(appUser.Img))
                        {
                            var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, appUser.Img.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        // Сохраняем новый файл
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(fileStream);
                        }

                        // Обновляем путь к изображению в базе данных
                        appUser.Img = $"/uploads/profiles/{uniqueFileName}";
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("ImageFile", "Ошибка при загрузке изображения. Пожалуйста, попробуйте снова.");
                        return View(model);
                    }
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
