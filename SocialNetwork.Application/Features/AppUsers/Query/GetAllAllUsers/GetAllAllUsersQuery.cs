using Elomoas.Application.Features.AppUsers.Query;
using MediatR;

namespace SocialNetwork.Application.Features.AppUsers.Query.GetAllAllUsers
{
    public record GetAllAllUsersQuery : IRequest<IEnumerable<AppUserDto>>;
} 