using Elomoas.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elomoas.Application.Features.Groups.Query.GetAll;

public class GetAllQueryHandler : IRequestHandler<GetAllQuery, IEnumerable<GetAllDto>>
{
    private readonly IGroupRepository _groupRepository;

    public GetAllQueryHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<IEnumerable<GetAllDto>> Handle(GetAllQuery query, CancellationToken cancellationToken)
    {
        var data = await _groupRepository.GetAllAsync();

        var result = data.Select(x => new GetAllDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Img = x.Img,
            PL = x.PL,
        });

        return result ?? new List<GetAllDto>() { };
    }
}
