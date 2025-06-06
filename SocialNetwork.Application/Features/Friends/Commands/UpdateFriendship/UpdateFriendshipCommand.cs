using MediatR;
using Elomoas.Domain.Entities;
using Elomoas.Domain.Entities.Enums;
using Elomoas.Application.Interfaces.Services;

namespace Elomoas.Application.Features.Friends.Commands.UpdateFriendship;

public record UpdateFriendshipCommand : IRequest<bool>
{
    public int Id { get; init; }
    public string UserId { get; init; }
    public string FriendId { get; init; }
    public FriendshipStatus Status { get; init; }
}

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