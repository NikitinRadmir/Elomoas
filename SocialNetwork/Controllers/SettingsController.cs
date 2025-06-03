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
using Elomoas.Application.Interfaces.Repositories;
using System.IO;

namespace Elomoas.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAuthService _authService;
        private readonly IAppUserRepository _userRepository;

        public SettingsController(
            UserManager<IdentityUser> userManager, 
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment,
            IAuthService authService,
            IAppUserRepository userRepository)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _authService = authService;
            _userRepository = userRepository;
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

        public async Task<IActionResult> Index()
        {
            var identityUser = await _userManager.GetUserAsync(User);
            if (identityUser == null)
                return RedirectToAction("Login", "Account");

            var appUsers = await _userRepository.GetAllUsersAsync();
            var currentUser = appUsers.FirstOrDefault(u => u.IdentityId == identityUser.Id);
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            var nameParts = currentUser.FullName?.Split(' ') ?? new string[0];
            var firstName = nameParts.Length > 0 ? nameParts[0] : string.Empty;
            var lastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;

            var viewModel = new SettingsVM
            {
                FirstName = firstName,
                LastName = lastName,
                Email = identityUser.Email,
                Description = currentUser.Description,
                ProfileImage = currentUser.ProfileImage
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(SettingsVM model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            var identityUser = await _userManager.GetUserAsync(User);
            if (identityUser == null)
                return RedirectToAction("Login", "Account");

            var appUsers = await _userRepository.GetAllUsersAsync();
            var currentUser = appUsers.FirstOrDefault(u => u.IdentityId == identityUser.Id);
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            if (model.Email != identityUser.Email)
            {
                var emailToken = await _userManager.GenerateChangeEmailTokenAsync(identityUser, model.Email);
                var emailResult = await _userManager.ChangeEmailAsync(identityUser, model.Email, emailToken);
                
                if (!emailResult.Succeeded)
                {
                    foreach (var error in emailResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View("Index", model);
                }
            }

            currentUser.Description = model.Description;

            if (!string.IsNullOrWhiteSpace(model.FirstName) || !string.IsNullOrWhiteSpace(model.LastName))
            {
                var fullName = $"{model.FirstName?.Trim()} {model.LastName?.Trim()}".Trim();
                currentUser.FullName = fullName;
            }

            if (model.ProfileImageFile != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(model.ProfileImageFile.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ProfileImageFile", "Поддерживаются только изображения форматов: .jpg, .jpeg, .png, .gif");
                    return View("Index", model);
                }

                if (model.ProfileImageFile.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("ProfileImageFile", "Размер файла не должен превышать 5MB");
                    return View("Index", model);
                }

                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "profiles");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                if (!string.IsNullOrEmpty(currentUser.ProfileImage))
                {
                    var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, currentUser.ProfileImage.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImageFile.CopyToAsync(stream);
                }

                currentUser.ProfileImage = $"/uploads/profiles/{fileName}";
            }

            await _userRepository.UpdateAsync(currentUser);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(PasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                var settingsVM = await GetCurrentSettingsVM();
                settingsVM.PasswordVM = model;
                return View("Index", settingsVM);
            }

            var identityUser = await _userManager.GetUserAsync(User);
            if (identityUser == null)
                return RedirectToAction("Login", "Account");

            var result = await _userManager.ChangePasswordAsync(identityUser, model.CurrentPassword, model.NewPassword);
            
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                
                var settingsVM = await GetCurrentSettingsVM();
                settingsVM.PasswordVM = model;
                return View("Index", settingsVM);
            }

            return RedirectToAction("Index");
        }

        private async Task<SettingsVM> GetCurrentSettingsVM()
        {
            var identityUser = await _userManager.GetUserAsync(User);
            var appUsers = await _userRepository.GetAllUsersAsync();
            var currentUser = appUsers.FirstOrDefault(u => u.IdentityId == identityUser.Id);

            var nameParts = currentUser.FullName?.Split(' ') ?? new string[0];
            var firstName = nameParts.Length > 0 ? nameParts[0] : string.Empty;
            var lastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;

            return new SettingsVM
            {
                FirstName = firstName,
                LastName = lastName,
                Email = identityUser.Email,
                Description = currentUser.Description,
                ProfileImage = currentUser.ProfileImage
            };
        }
    }
}
