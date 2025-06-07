using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Elomoas.Application.Features.Friends.Queries.GetFriendship;

public class GetFriendshipQueryHandler : IRequestHandler<GetFriendshipQuery, Friendship>
{
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly ILogger<GetFriendshipQueryHandler> _logger;

    public GetFriendshipQueryHandler(
        IFriendshipRepository friendshipRepository,
        ILogger<GetFriendshipQueryHandler> logger)
    {
        _friendshipRepository = friendshipRepository;
        _logger = logger;
    }

    public async Task<Friendship> Handle(GetFriendshipQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await _friendshipRepository.GetFriendshipAsync(request.UserId, request.FriendId);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error getting friendship status between users {UserId} and {FriendId}", 
                request.UserId, request.FriendId);
            throw;
        }
    }
} 