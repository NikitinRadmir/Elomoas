using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Domain.Entities;

namespace SocialNetwork.Application.Features.AppUsers.Command.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAppUserRepository _userRepository;
        private readonly ILogger<CreateUserCommandHandler> _logger;

        public CreateUserCommandHandler(
            UserManager<IdentityUser> userManager,
            IAppUserRepository userRepository,
            ILogger<CreateUserCommandHandler> logger)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var identityUser = new IdentityUser { UserName = request.Email, Email = request.Email };
                var identityResult = await _userManager.CreateAsync(identityUser, request.Password);

                if (!identityResult.Succeeded)
                {
                    foreach (var error in identityResult.Errors)
                    {
                        _logger.LogWarning("Identity creation failed: {Error}", error.Description);
                    }
                    return false;
                }

                var appUser = new AppUser
                {
                    IdentityId = identityUser.Id,
                    Name = request.Name,
                    Email = request.Email,
                    Password = request.Password,
                    Description = request.Description,
                    Img = request.Img ?? "/uploads/profiles/default-icon.jpg"
                };

                await _userRepository.UpdateAsync(appUser);
                _logger.LogInformation("Successfully created user {Email}", request.Email);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user {Email}", request.Email);
                return false;
            }
        }
    }
} 