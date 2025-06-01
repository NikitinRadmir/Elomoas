using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Application.Interfaces.Services;
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
    private readonly IGroupSubscriptionRepository _subscriptionRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetAllQueryHandler(
        IGroupRepository groupRepository,
        IGroupSubscriptionRepository subscriptionRepository,
        ICurrentUserService currentUserService)
    {
        _groupRepository = groupRepository;
        _subscriptionRepository = subscriptionRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<GetAllDto>> Handle(GetAllQuery query, CancellationToken cancellationToken)
    {
        var data = await _groupRepository.GetAllAsync();
        var userId = _currentUserService.UserId;

        var result = new List<GetAllDto>();
        foreach (var group in data)
        {
            var isSubscribed = userId.HasValue && await _subscriptionRepository.IsSubscribed(userId.Value, group.Id);

            result.Add(new GetAllDto
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
                Img = group.Img,
                PL = group.PL,
                IsCurrentUserSubscribed = isSubscribed
            });
        }

        return result;
    }
}
