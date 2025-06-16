using System.Collections.Generic;
using System.Linq;
using Elomoas.Domain.Entities;

namespace Elomoas.Application.Features.Friends.Dtos;

public static class FriendshipMapper
{
    public static FriendshipDto ToDto(this Friendship friendship)
    {
        if (friendship == null) return null;

        return new FriendshipDto
        {
            Id = friendship.Id,
            UserId = friendship.UserId,
            FriendId = friendship.FriendId,
            Status = friendship.Status,
        };
    }

    public static IEnumerable<FriendshipDto> ToDtos(this IEnumerable<Friendship> friendships)
    {
        if (friendships == null) return Enumerable.Empty<FriendshipDto>();

        return friendships.Select(f => f.ToDto());
    }
} 