using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Features.Friends.Dtos;
using Elomoas.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Elomoas.Application.Features.Friends.Queries.GetFriendshipById;

public class GetFriendshipByIdQueryHandler : IRequestHandler<GetFriendshipByIdQuery, FriendshipDto>
{
    private readonly IFriendshipService _friendshipService;
    private readonly ILogger<GetFriendshipByIdQueryHandler> _logger;

    public GetFriendshipByIdQueryHandler(
        IFriendshipService friendshipService,
        ILogger<GetFriendshipByIdQueryHandler> logger)
    {
        _friendshipService = friendshipService;
        _logger = logger;
    }

    public async Task<FriendshipDto> Handle(GetFriendshipByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var friendship = await _friendshipService.GetFriendshipByIdAsync(request.Id);
            if (friendship == null)
            {
                _logger.LogWarning("Friendship with ID {Id} not found", request.Id);
                return null;
            }

            var dto = new FriendshipDto
            {
                UserId = friendship.UserId,
                FriendId = friendship.FriendId,
                Status = friendship.Status
            };

            _logger.LogInformation("Retrieved friendship {Id} between users {UserId} and {FriendId} with status {Status}", 
                request.Id, dto.UserId, dto.FriendId, dto.Status);

            return dto;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error getting friendship with ID {Id}", request.Id);
            return null;
        }
    }
} 