using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Elomoas.Application.Interfaces.Repositories;

namespace SocialNetwork.Application.Features.AppUsers.Command.UpdateUserPassword;

public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand, bool>
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<UpdateUserPasswordCommandHandler> _logger;

    public UpdateUserPasswordCommandHandler(
        IAppUserRepository appUserRepository,
        UserManager<IdentityUser> userManager,
        ILogger<UpdateUserPasswordCommandHandler> logger)
    {
        _appUserRepository = appUserRepository;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Получаем AppUser с включенным IdentityUser
            var appUser = await _appUserRepository.GetUserByIdWithIdentityAsync(request.AppUserId);
            if (appUser == null)
            {
                _logger.LogError("AppUser not found for ID: {UserId}", request.AppUserId);
                return false;
            }

            // Получаем IdentityUser
            var identityUser = await _userManager.FindByIdAsync(appUser.IdentityId);
            if (identityUser == null)
            {
                _logger.LogError("IdentityUser not found for IdentityId: {IdentityId}", appUser.IdentityId);
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

            var updateResult = await _userManager.UpdateAsync(identityUser);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    _logger.LogError("Failed to update IdentityUser password: {Error}", error.Description);
                }
                return false;
            }

            // Обновляем пароль в AppUser (хотя это поле не должно использоваться для аутентификации)
            appUser.Password = request.NewPassword; // Это поле оставлено для обратной совместимости
            await _appUserRepository.UpdateAsync(appUser);

            _logger.LogInformation("Successfully updated password for user {UserId}", request.AppUserId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating password for user {UserId}", request.AppUserId);
            return false;
        }
    }
} 