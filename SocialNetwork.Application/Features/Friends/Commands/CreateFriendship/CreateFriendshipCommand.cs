using MediatR;
using Elomoas.Domain.Entities;
using Elomoas.Domain.Entities.Enums;
using System;

namespace Elomoas.Application.Features.Friends.Commands.CreateFriendship;

public record CreateFriendshipCommand : IRequest<Friendship>
{
    public string UserId { get; init; }
    public string FriendId { get; init; }
    public FriendshipStatus Status { get; init; }
} 