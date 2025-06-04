using MediatR;
using System.Collections.Generic;
using Elomoas.Domain.Entities;

namespace Elomoas.Application.Features.Friends.Queries.GetPendingFriendRequests
{
    public class GetPendingFriendRequestsQuery : IRequest<IEnumerable<Friendship>>
    {
        public string UserId { get; set; }
    }
} 