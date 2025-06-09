using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Friends.Commands.UpdateFriendship;

public class UpdateFriendshipCommandHandler : IRequestHandler<UpdateFriendshipCommand, bool>
{
    private readonly IFriendshipService _friendshipService;

    public UpdateFriendshipCommandHandler(IFriendshipService friendshipService)
    {
        _friendshipService = friendshipService;
    }

    public async Task<bool> Handle(UpdateFriendshipCommand request, CancellationToken cancellationToken)
    {
        var friendship = new Friendship
        {
            Id = request.Id,
            UserId = request.UserId,
            FriendId = request.FriendId,
            Status = request.Status
        };

        return await _friendshipService.UpdateFriendshipAsync(friendship);
    }
} 