using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Application.Features.Auth.Query.GetIdentityUserById;

namespace SocialNetwork.Application.Features.AppUsers.Command.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUserService _userService;
    private readonly IMediator _mediator;
    private readonly IAppUserRepository _appUserRepository;
    private readonly ILogger<UpdateUserCommandHandler> _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public UpdateUserCommandHandler(
        IUserService userService,
        IMediator mediator,
        IAppUserRepository appUserRepository,
        ILogger<UpdateUserCommandHandler> logger,
        UserManager<IdentityUser> userManager)
    {
        _userService = userService;
        _mediator = mediator;
        _appUserRepository = appUserRepository;
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var appUser = await _appUserRepository.GetUserByIdWithIdentityAsync(request.Id);
            if (appUser == null)
            {
                _logger.LogError("AppUser not found for ID: {UserId}", request.Id);
                return false;
            }

            appUser.Name = request.Name;
            appUser.Email = request.Email;
            appUser.Description = request.Description;
            appUser.Img = request.Img;

            var identityUser = await _mediator.Send(new GetIdentityUserByIdQuery(appUser.IdentityId));
            if (identityUser == null)
            {
                _logger.LogError("IdentityUser not found for ID: {IdentityId}", appUser.IdentityId);
                return false;
            }

            // Смена пароля, если задан новый
            if (!string.IsNullOrEmpty(request.NewPassword))
            {
                var passwordValidator = new PasswordValidator<IdentityUser>();
                var validationResult = await passwordValidator.ValidateAsync(_userManager, identityUser, request.NewPassword);
                if (!validationResult.Succeeded)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        _logger.LogWarning("Password validation failed: {Error}", error.Description);
                    }
                    return false;
                }
                identityUser.PasswordHash = _userManager.PasswordHasher.HashPassword(identityUser, request.NewPassword);
                var updateIdentityResult = await _userManager.UpdateAsync(identityUser);
                if (!updateIdentityResult.Succeeded)
                {
                    foreach (var error in updateIdentityResult.Errors)
                    {
                        _logger.LogError("Failed to update IdentityUser password: {Error}", error.Description);
                    }
                    return false;
                }
                appUser.Password = request.NewPassword;
            }

            var updateResult = await _userService.UpdateUserAsync(appUser);
            if (!updateResult)
            {
                _logger.LogError("Failed to update AppUser with ID: {UserId}", request.Id);
                return false;
            }

            _logger.LogInformation("Successfully updated user with ID: {UserId}", request.Id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with ID: {UserId}", request.Id);
            return false;
        }
    }
} 