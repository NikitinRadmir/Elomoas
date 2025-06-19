using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;
using Elomoas.Application.Features.Groups.Query.GetAll;
using System.Linq;

namespace Elomoas.Application.Features.Groups.Queries;

public class GetGroupByIdQueryHandler : IRequestHandler<GetGroupByIdQuery, GroupDto?>
{
    private readonly IGroupService _groupService;

    public GetGroupByIdQueryHandler(IGroupService groupService)
    {
        _groupService = groupService;
    }

    public async Task<GroupDto?> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken)
    {
        var data = await _groupService.GetGroupByIdAsync(request.Id);
        var res = new GroupDto
        {
            Id = data.Id,
            Name = data.Name,
            Description = data.Description,
            Img = data.Img,
            PL = data.PL,

        };

        return res;
    }
} 