using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Features.Friends.Dtos;
using Elomoas.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Elomoas.Application.Features.Friends.Queries.GetFriendship;

public class GetFriendshipQueryHandler : IRequestHandler<GetFriendshipQuery, FriendshipDto>
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

    public async Task<FriendshipDto> Handle(GetFriendshipQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var friendship = await _friendshipRepository.GetFriendshipAsync(request.UserId, request.FriendId);
            if (friendship == null)
            {
                _logger.LogInformation("No friendship found between users {UserId} and {FriendId}", 
                    request.UserId, request.FriendId);
                return null;
            }

            var dto = new FriendshipDto
            {
                UserId = friendship.UserId,
                FriendId = friendship.FriendId,
                Status = friendship.Status
            };

            _logger.LogInformation("Retrieved friendship between users {UserId} and {FriendId} with status {Status}", 
                dto.UserId, dto.FriendId, dto.Status);

            return dto;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error getting friendship status between users {UserId} and {FriendId}", 
                request.UserId, request.FriendId);
            return null;
        }
    }
} 