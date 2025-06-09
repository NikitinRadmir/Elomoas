using MediatR;

namespace Elomoas.Application.Features.Auth.Query
{
    public record GetCurrentUserQuery() : IRequest<string>;
} 