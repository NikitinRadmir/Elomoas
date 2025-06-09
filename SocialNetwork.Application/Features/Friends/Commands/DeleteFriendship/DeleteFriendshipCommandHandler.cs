using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using MediatR;

namespace Elomoas.Application.Features.Friends.Commands.DeleteFriendship;

public class DeleteFriendshipCommandHandler : IRequestHandler<DeleteFriendshipCommand, bool>
{
    private readonly IFriendshipService _friendshipService;

    public DeleteFriendshipCommandHandler(IFriendshipService friendshipService)
    {
        _friendshipService = friendshipService;
    }

    public async Task<bool> Handle(DeleteFriendshipCommand request, CancellationToken cancellationToken)
    {
        return await _friendshipService.DeleteFriendshipAsync(request.Id);
    }
} 