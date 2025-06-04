using MediatR;
using System.Collections.Generic;
using Elomoas.Application.Features.AppUsers.Query;

namespace Elomoas.Application.Features.Friends.Queries.GetUserFriends
{
    public class GetUserFriendsQuery : IRequest<IEnumerable<AppUserDto>>
    {
        public string UserId { get; set; }
    }
} 