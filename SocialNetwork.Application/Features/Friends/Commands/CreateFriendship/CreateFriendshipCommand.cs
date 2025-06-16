using MediatR;
using Elomoas.Domain.Entities.Enums;

namespace Elomoas.Application.Features.Friends.Commands.CreateFriendship;

public record CreateFriendshipCommand : IRequest<bool>
{
    public string UserId { get; init; }
    public string FriendId { get; init; }
    public FriendshipStatus Status { get; init; }
} 