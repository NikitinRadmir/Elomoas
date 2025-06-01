using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Domain.Entities;
using Elomoas.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Elomoas.Application.Features.Groups.Queries.GetSubscribedGroups
{
    public class GetSubscribedGroupsQueryHandler : IRequestHandler<GetSubscribedGroupsQuery, IEnumerable<Group>>
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

        public async Task<IEnumerable<Group>> Handle(GetSubscribedGroupsQuery request, CancellationToken cancellationToken)
        {
            var subscribedGroupIds = await _subscriptionRepository.Entities
                .Where(s => s.UserId == request.UserId)
                .Select(s => s.GroupId)
                .ToListAsync(cancellationToken);

            var subscribedGroups = await _groupRepository.Entities
                .Where(g => subscribedGroupIds.Contains(g.Id))
                .ToListAsync(cancellationToken);

            return subscribedGroups;
        }
    }
} 