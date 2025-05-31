using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elomoas.Application.Features.Auth.Command
{
    public class LoginCommand : IRequest<bool>
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
