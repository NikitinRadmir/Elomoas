using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Groups.Queries;

public class GetGroupByIdQueryHandler : IRequestHandler<GetGroupByIdQuery, Group>
{
    private readonly IGroupService _groupService;

    public GetGroupByIdQueryHandler(IGroupService groupService)
    {
        _groupService = groupService;
    }

    public async Task<Group> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken)
    {
        return await _groupService.GetGroupByIdAsync(request.Id);
    }
} 