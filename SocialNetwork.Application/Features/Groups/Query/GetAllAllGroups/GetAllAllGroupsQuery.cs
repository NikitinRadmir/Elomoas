using MediatR;
using Elomoas.Application.Features.Groups.Query.GetAll;

namespace SocialNetwork.Application.Features.Groups.Query.GetAllAllGroups
{
    public class GetAllAllGroupsQuery : IRequest<IEnumerable<GetAllDto>>
    {
    }
} 