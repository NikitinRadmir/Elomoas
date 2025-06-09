using MediatR;

namespace Elomoas.Application.Features.Auth.Command
{
    public record LogoutCommand : IRequest<bool>;
} 