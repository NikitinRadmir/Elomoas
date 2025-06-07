using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Elomoas.Application.Interfaces.Repositories;

namespace SocialNetwork.Application.Features.AppUsers.Command.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUserService _userService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IAppUserRepository _appUserRepository;
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(
        IUserService userService,
        UserManager<IdentityUser> userManager,
        IAppUserRepository appUserRepository,
        ILogger<UpdateUserCommandHandler> logger)
    {
        _userService = userService;
        _userManager = userManager;
        _appUserRepository = appUserRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Получаем пользователя с включенным IdentityUser
            var appUser = await _appUserRepository.GetUserByIdWithIdentityAsync(request.Id);
            if (appUser == null)
            {
                _logger.LogError("AppUser not found for ID: {UserId}", request.Id);
                return false;
            }

            // Обновляем основные данные
            appUser.Name = request.Name;
            appUser.Email = request.Email;
            appUser.Description = request.Description;
            appUser.Img = request.Img;

            // Если указан новый пароль, обновляем его
            if (!string.IsNullOrEmpty(request.NewPassword))
            {
                var identityUser = await _userManager.FindByIdAsync(appUser.IdentityId);
                if (identityUser == null)
                {
                    _logger.LogError("IdentityUser not found for ID: {IdentityId}", appUser.IdentityId);
                    return false;
                }

                // Проверяем валидность пароля
                var passwordValidator = new PasswordValidator<IdentityUser>();
                var validationResult = await passwordValidator.ValidateAsync(_userManager, null, request.NewPassword);
                if (!validationResult.Succeeded)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        _logger.LogWarning("Password validation failed: {Error}", error.Description);
                    }
                    return false;
                }

                // Обновляем пароль в IdentityUser
                var newPasswordHash = _userManager.PasswordHasher.HashPassword(identityUser, request.NewPassword);
                identityUser.PasswordHash = newPasswordHash;

                var identityResult = await _userManager.UpdateAsync(identityUser);
                if (!identityResult.Succeeded)
                {
                    foreach (var error in identityResult.Errors)
                    {
                        _logger.LogError("Failed to update IdentityUser password: {Error}", error.Description);
                    }
                    return false;
                }

                // Обновляем пароль в AppUser для обратной совместимости
                appUser.Password = request.NewPassword;
            }

            // Сохраняем изменения в AppUser
            var updateResult = await _userService.UpdateUserAsync(appUser);
            if (!updateResult)
            {
                _logger.LogError("Failed to update AppUser");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user {UserId}", request.Id);
            return false;
        }
    }
} 