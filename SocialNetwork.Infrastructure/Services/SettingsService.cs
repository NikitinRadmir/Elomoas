using System;
using System.Linq;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Application.Features.Settings.Dto;
using Elomoas.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Elomoas.Infrastructure.Services;

public class SettingsService : ISettingsService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SettingsService> _logger;

    public SettingsService(UserManager<IdentityUser> userManager, ApplicationDbContext context, ILogger<SettingsService> logger)
    {
        _userManager = userManager;
        _context = context;
        _logger = logger;
    }

    public async Task<AccountInfoDto?> GetAccountInfoAsync(string identityId)
    {
        var appUser = await _context.AppUsers.FirstOrDefaultAsync(u => u.IdentityId == identityId);
        if (appUser == null) return null;
        return new AccountInfoDto
        {
            FirstName = appUser.Name?.Split(' ').FirstOrDefault() ?? "",
            LastName = appUser.Name?.Contains(' ') == true ? string.Join(" ", appUser.Name.Split(' ').Skip(1)) : "",
            Email = appUser.Email,
            Description = appUser.Description,
            Img = appUser.Img
        };
    }

    public async Task<bool> UpdateAccountInfoAsync(string identityId, AccountInfoDto model, string? base64Image, string webRootPath)
    {
        var identityUser = await _userManager.FindByIdAsync(identityId);
        if (identityUser == null) return false;

        if (!string.IsNullOrEmpty(model.Email) && identityUser.Email != model.Email)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null) return false;
            identityUser.Email = model.Email;
            identityUser.UserName = model.Email;
            var identityResult = await _userManager.UpdateAsync(identityUser);
            if (!identityResult.Succeeded) return false;
        }

        var appUser = await _context.AppUsers.FirstOrDefaultAsync(u => u.IdentityId == identityId);
        if (appUser == null) return false;

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
        if (!string.IsNullOrEmpty(base64Image))
        {
            try
            {
                var uploadsFolder = Path.Combine(webRootPath, "uploads", "profiles");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = $"{Guid.NewGuid()}.jpg";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                var imageBytes = Convert.FromBase64String(base64Image);
                await File.WriteAllBytesAsync(filePath, imageBytes);
                appUser.Img = $"/uploads/profiles/{uniqueFileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when loading the image for user {IdentityId}", identityId);
                return false;
            }
        }
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ChangePasswordAsync(string identityId, string currentPassword, string newPassword)
    {
        var identityUser = await _userManager.FindByIdAsync(identityId);
        if (identityUser == null) return false;
        var result = await _userManager.ChangePasswordAsync(identityUser, currentPassword, newPassword);
        if (!result.Succeeded) return false;
        var appUser = await _context.AppUsers.FirstOrDefaultAsync(u => u.IdentityId == identityId);
        if (appUser != null)
        {
            appUser.Password = newPassword;
            await _context.SaveChangesAsync();
        }
        return true;
    }
} 