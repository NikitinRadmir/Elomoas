using MediatR;
using Elomoas.Domain.Entities;
using Elomoas.Domain.Entities.Enums;

namespace Elomoas.Application.Features.Friends.Commands.UpdateFriendship;

public record UpdateFriendshipCommand : IRequest<bool>
{
    public int Id { get; init; }
    public string UserId { get; init; }
    public string FriendId { get; init; }
    public FriendshipStatus Status { get; init; }
} 