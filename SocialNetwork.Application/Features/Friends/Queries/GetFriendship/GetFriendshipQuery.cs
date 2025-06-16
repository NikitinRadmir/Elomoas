using Elomoas.Application.Features.Friends.Dtos;
using MediatR;

namespace Elomoas.Application.Features.Friends.Queries.GetFriendship;

public record GetFriendshipQuery : IRequest<FriendshipDto>
{
    public string UserId { get; init; }
    public string FriendId { get; init; }

    public GetFriendshipQuery(string userId, string friendId)
    {
        UserId = userId;
        FriendId = friendId;
    }
} 