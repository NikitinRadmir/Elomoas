using MediatR;
using Elomoas.Application.Features.Friends.Dtos;

namespace Elomoas.Application.Features.Friends.Queries.GetFriendshipById;

public record GetFriendshipByIdQuery(int Id) : IRequest<FriendshipDto>; 