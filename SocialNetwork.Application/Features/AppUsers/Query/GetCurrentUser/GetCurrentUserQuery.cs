using MediatR;
using Elomoas.Application.Features.AppUsers.Query;

namespace Elomoas.Application.Features.AppUsers.Query.GetCurrentUser
{
    public record GetCurrentUserQuery() : IRequest<AppUserDto>;
} 