using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;

namespace Elomoas.Application.Features.Auth.Command
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
    {
        private readonly IAuthService _authService;

        public LogoutCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            await _authService.LogoutAsync();
            return true;
        }
    }
} 