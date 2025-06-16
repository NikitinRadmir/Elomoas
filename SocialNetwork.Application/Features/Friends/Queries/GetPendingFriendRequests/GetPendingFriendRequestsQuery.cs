using MediatR;
using System.Collections.Generic;
using Elomoas.Application.Features.Friends.Dtos;

namespace Elomoas.Application.Features.Friends.Queries.GetPendingFriendRequests
{
    public class GetPendingFriendRequestsQuery : IRequest<IEnumerable<FriendshipDto>>
    {
        public string UserId { get; private set; }

        public GetPendingFriendRequestsQuery(string userId)
        {
            UserId = userId;
        }
    }
} 