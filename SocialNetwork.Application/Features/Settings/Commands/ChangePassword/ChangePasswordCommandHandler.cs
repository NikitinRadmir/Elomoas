using MediatR;
using Microsoft.AspNetCore.Identity;
using Elomoas.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Elomoas.Application.Features.Settings.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAppUserRepository _userRepository;

        public ChangePasswordCommandHandler(
            UserManager<IdentityUser> userManager,
            IAppUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var identityUser = await _userManager.FindByIdAsync(request.IdentityId);
            if (identityUser == null)
                return false;

            var result = await _userManager.ChangePasswordAsync(
                identityUser, 
                request.CurrentPassword, 
                request.NewPassword);

            if (!result.Succeeded)
                return false;

            var appUser = await _userRepository.GetByIdentityIdAsync(request.IdentityId);
            if (appUser != null)
            {
                appUser.Password = request.NewPassword;
                await _userRepository.UpdateAsync(appUser);
            }

            return true;
        }
    }
} 