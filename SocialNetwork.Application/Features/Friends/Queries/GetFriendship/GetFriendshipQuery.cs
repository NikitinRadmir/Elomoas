using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Friends.Queries.GetFriendship;

public record GetFriendshipQuery(string UserId, string FriendId) : IRequest<Friendship>; 