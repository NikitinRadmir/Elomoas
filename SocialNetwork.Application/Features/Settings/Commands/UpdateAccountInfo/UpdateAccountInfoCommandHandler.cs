using MediatR;
using Microsoft.AspNetCore.Identity;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Application.Interfaces.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Elomoas.Application.Features.Settings.Commands.UpdateAccountInfo
{
    public class UpdateAccountInfoCommandHandler : IRequestHandler<UpdateAccountInfoCommand, bool>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAppUserRepository _userRepository;
        private readonly IFileService _fileService;

        public UpdateAccountInfoCommandHandler(
            UserManager<IdentityUser> userManager,
            IAppUserRepository userRepository,
            IFileService fileService)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _fileService = fileService;
        }

        public async Task<bool> Handle(UpdateAccountInfoCommand request, CancellationToken cancellationToken)
        {
            var identityUser = await _userManager.FindByIdAsync(request.IdentityId);
            if (identityUser == null)
                return false;

            if (!string.IsNullOrEmpty(request.Email) && identityUser.Email != request.Email)
            {
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                    return false;

                identityUser.Email = request.Email;
                identityUser.UserName = request.Email;
                var identityResult = await _userManager.UpdateAsync(identityUser);

                if (!identityResult.Succeeded)
                    return false;
            }

            var appUser = await _userRepository.GetByIdentityIdAsync(request.IdentityId);
            if (appUser == null)
                return false;

            if (!string.IsNullOrEmpty(request.FirstName) || !string.IsNullOrEmpty(request.LastName))
            {
                var fullName = string.IsNullOrWhiteSpace(request.LastName)
                    ? request.FirstName
                    : $"{request.FirstName} {request.LastName}";
                appUser.Name = fullName?.Trim();
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                appUser.Email = request.Email;
            }

            if (request.Description != null)
            {
                appUser.Description = request.Description;
            }

            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                var newImagePath = await _fileService.SaveProfileImageAsync(request.ImageFile, appUser.Img);
                if (newImagePath == null)
                    return false;

                appUser.Img = newImagePath;
            }

            await _userRepository.UpdateAsync(appUser);
            return true;
        }
    }
} 