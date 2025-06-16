using System;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Elomoas.Application.Features.Friends.Commands.CreateFriendship;

public class CreateFriendshipCommandHandler : IRequestHandler<CreateFriendshipCommand, bool>
{
    private readonly IFriendshipService _friendshipService;
    private readonly ILogger<CreateFriendshipCommandHandler> _logger;

    public CreateFriendshipCommandHandler(
        IFriendshipService friendshipService,
        ILogger<CreateFriendshipCommandHandler> logger)
    {
        _friendshipService = friendshipService;
        _logger = logger;
    }

    public async Task<bool> Handle(CreateFriendshipCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating friendship. UserId: {UserId}, FriendId: {FriendId}, Status: {Status}",
            request.UserId, request.FriendId, request.Status);

        return await _friendshipService.CreateFriendshipAsync(
            request.UserId,
            request.FriendId,
            request.Status);
    }
} 