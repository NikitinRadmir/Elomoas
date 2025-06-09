using MediatR;
using Elomoas.Domain.Entities;

namespace Elomoas.Application.Features.Friends.Queries.GetFriendshipById;

public record GetFriendshipByIdQuery(int Id) : IRequest<Friendship>; 