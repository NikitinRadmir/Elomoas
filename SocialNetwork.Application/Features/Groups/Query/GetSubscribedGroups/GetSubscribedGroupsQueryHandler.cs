using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Domain.Entities;
using Elomoas.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Elomoas.Application.Features.Groups.Query.GetAll;

namespace Elomoas.Application.Features.Groups.Query.GetSubscribedGroups
{
    public class GetSubscribedGroupsQueryHandler : IRequestHandler<GetSubscribedGroupsQuery, IEnumerable<GroupDto>>
    {
        private readonly IGenericRepository<GroupSubscription> _subscriptionRepository;
        private readonly IGenericRepository<Group> _groupRepository;

        public GetSubscribedGroupsQueryHandler(
            IGenericRepository<GroupSubscription> subscriptionRepository,
            IGenericRepository<Group> groupRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _groupRepository = groupRepository;
        }

        public async Task<IEnumerable<GroupDto>> Handle(GetSubscribedGroupsQuery request, CancellationToken cancellationToken)
        {
            var subscribedGroupIds = await _subscriptionRepository.Entities
                .Where(s => s.UserId == request.UserId)
                .Select(s => s.GroupId)
                .ToListAsync(cancellationToken);

            var subscribedGroups = await _groupRepository.Entities
                .Where(g => subscribedGroupIds.Contains(g.Id))
                .ToListAsync(cancellationToken);

            return subscribedGroups.Select(group => new GroupDto
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
                Img = group.Img ?? "/images/default-icon.jpg",
                PL = group.PL,
                IsCurrentUserSubscribed = true
            });
        }
    }
}