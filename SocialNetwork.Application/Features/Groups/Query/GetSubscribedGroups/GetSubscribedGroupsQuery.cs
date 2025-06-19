using MediatR;
using System.Collections.Generic;
using Elomoas.Domain.Entities;
using Elomoas.Application.Features.Groups.Query.GetAll;

namespace Elomoas.Application.Features.Groups.Query.GetSubscribedGroups
{
    public class GetSubscribedGroupsQuery : IRequest<IEnumerable<GroupDto>>
    {
        public int UserId { get; set; }

        public GetSubscribedGroupsQuery(int userId)
        {
            UserId = userId;
        }
    }
}