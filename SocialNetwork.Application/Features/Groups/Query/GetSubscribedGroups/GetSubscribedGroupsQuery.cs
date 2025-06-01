using MediatR;
using System.Collections.Generic;
using Elomoas.Domain.Entities;

namespace Elomoas.Application.Features.Groups.Query.GetSubscribedGroups
{
    public class GetSubscribedGroupsQuery : IRequest<IEnumerable<Group>>
    {
        public int UserId { get; set; }

        public GetSubscribedGroupsQuery(int userId)
        {
            UserId = userId;
        }
    }
}