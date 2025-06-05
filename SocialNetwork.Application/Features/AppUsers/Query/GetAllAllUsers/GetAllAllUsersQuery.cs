using Elomoas.Application.Features.AppUsers.Query;
using MediatR;

namespace SocialNetwork.Application.Features.AppUsers.Query.GetAllAllUsers
{
    public class GetAllAllUsersQuery : IRequest<IEnumerable<AppUserDto>>
    {
    }
} 