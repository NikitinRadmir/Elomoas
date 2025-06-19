using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using Microsoft.Extensions.Logging;
using Elomoas.Application.Interfaces.Repositories;

namespace Elomoas.Application.Features.Auth.Query
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
    {
        private readonly IAuthService _authService;

        public RegisterCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<bool> Handle(RegisterCommand command, CancellationToken cancellationToken) =>
            await _authService.RegisterAsync(command.Name, command.Email, command.Password);
    }
}
