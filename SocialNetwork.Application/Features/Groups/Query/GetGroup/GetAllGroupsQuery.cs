using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Groups.Queries;

public record GetAllGroupsQuery : IRequest<IEnumerable<Group>>;

public class GetAllGroupsQueryHandler : IRequestHandler<GetAllGroupsQuery, IEnumerable<Group>>
{
    private readonly IGroupService _groupService;

    public GetAllGroupsQueryHandler(IGroupService groupService)
    {
        _groupService = groupService;
    }

    public async Task<IEnumerable<Group>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
    {
        return await _groupService.GetAllGroupsAsync();
    }
} 