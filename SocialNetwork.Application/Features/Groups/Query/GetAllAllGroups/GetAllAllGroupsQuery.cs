using MediatR;
using Elomoas.Application.Features.Groups.Query.GetAll;

namespace SocialNetwork.Application.Features.Groups.Query.GetAllAllGroups
{
    public record GetAllAllGroupsQuery : IRequest<IEnumerable<GetAllDto>>;
} 