using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;
using Elomoas.Application.Features.Groups.Query.GetAll;

namespace Elomoas.Application.Features.Groups.Queries;

public class GetAllGroupsQueryHandler : IRequestHandler<GetAllGroupsQuery, IEnumerable<GroupDto>>
{
    private readonly IGroupService _groupService;

    public GetAllGroupsQueryHandler(IGroupService groupService)
    {
        _groupService = groupService;
    }

    public async Task<IEnumerable<GroupDto>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
    {
        return await _groupService.GetAllGroupsAsync();
    }
} 