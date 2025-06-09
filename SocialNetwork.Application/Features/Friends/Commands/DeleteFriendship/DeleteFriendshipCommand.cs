using MediatR;

namespace Elomoas.Application.Features.Friends.Commands.DeleteFriendship;

public record DeleteFriendshipCommand(int Id) : IRequest<bool>; 