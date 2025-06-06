using MediatR;

namespace Elomoas.Application.Features.Settings.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest<bool>
    {
        public string IdentityId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

        public ChangePasswordCommand(string identityId, string currentPassword, string newPassword)
        {
            IdentityId = identityId;
            CurrentPassword = currentPassword;
            NewPassword = newPassword;
        }
    }
} 