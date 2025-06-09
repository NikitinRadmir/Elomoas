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

    public UpdateUserCommandHandler(
        IUserService userService,
        IMediator mediator,
        IAppUserRepository appUserRepository,
        ILogger<UpdateUserCommandHandler> logger)
    {
        _userService = userService;
        _mediator = mediator;
        _appUserRepository = appUserRepository;
        _logger = logger;
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