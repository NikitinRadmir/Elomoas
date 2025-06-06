using MediatR;
using Elomoas.Domain.Entities;
using Elomoas.Domain.Entities.Enums;
using Elomoas.Application.Interfaces.Services;
using System;

namespace Elomoas.Application.Features.Friends.Commands.CreateFriendship;

public record CreateFriendshipCommand : IRequest<Friendship>
{
    public string UserId { get; init; }
    public string FriendId { get; init; }
    public FriendshipStatus Status { get; init; }
}

public class CreateFriendshipCommandHandler : IRequestHandler<CreateFriendshipCommand, Friendship>
{
    private readonly IFriendshipService _friendshipService;

    public CreateFriendshipCommandHandler(IFriendshipService friendshipService)
    {
        _friendshipService = friendshipService;
    }

    public async Task<Friendship> Handle(CreateFriendshipCommand request, CancellationToken cancellationToken)
    {
        var friendship = new Friendship
        {
            UserId = request.UserId,
            FriendId = request.FriendId,
            Status = request.Status,
            AddedAt = DateTime.UtcNow
        };

        return await _friendshipService.CreateFriendshipAsync(friendship);
    }
} 