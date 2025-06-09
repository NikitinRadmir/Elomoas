using System;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Friends.Commands.CreateFriendship;

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