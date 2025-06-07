using Elomoas.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elomoas.Application.Features.Auth.Command
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, bool>
    {
        private readonly IAuthService _authService;

        public LoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<bool> Handle(LoginCommand command, CancellationToken cancellationToken) =>
            await _authService.LoginAsync(command.Email, command.Password);
    }

}
