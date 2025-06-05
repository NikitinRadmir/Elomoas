using MediatR;
using Elomoas.Application.Features.Groups.Query.GetAll;
using Elomoas.Application.Interfaces.Repositories;

namespace SocialNetwork.Application.Features.Groups.Query.GetAllAllGroups
{
    public class GetAllAllGroupsQueryHandler : IRequestHandler<GetAllAllGroupsQuery, IEnumerable<GetAllDto>>
    {
        private readonly IGroupRepository _groupRepository;

        public GetAllAllGroupsQueryHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<IEnumerable<GetAllDto>> Handle(GetAllAllGroupsQuery request, CancellationToken cancellationToken)
        {
            var groups = await _groupRepository.GetAllAsync();
            
            return groups.Select(group => new GetAllDto
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
                Img = group.Img ?? "/images/default-icon.jpg",
                PL = group.PL,
                IsCurrentUserSubscribed = false // В админке это поле не используется
            });
        }
    }
} 