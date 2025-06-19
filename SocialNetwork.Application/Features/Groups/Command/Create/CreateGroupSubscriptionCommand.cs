using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Elomoas.Application.Features.Groups.Commands;

public record CreateGroupSubscriptionCommand : IRequest<bool>
{
    public int UserId { get; init; }
    public int GroupId { get; init; }

    public CreateGroupSubscriptionCommand(int userId, int groupId)
    {
        UserId = userId;
        GroupId = groupId;
    }
}

public class CreateGroupSubscriptionCommandHandler : IRequestHandler<CreateGroupSubscriptionCommand, bool>
{
    private readonly IGroupSubscriptionService _subscriptionService;
    private readonly ILogger<CreateGroupSubscriptionCommandHandler> _logger;

    public CreateGroupSubscriptionCommandHandler(
        IGroupSubscriptionService subscriptionService,
        ILogger<CreateGroupSubscriptionCommandHandler> logger)
    {
        _subscriptionService = subscriptionService;
        _logger = logger;
    }

    public async Task<bool> Handle(CreateGroupSubscriptionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Creating subscription for user {UserId} to group {GroupId}", 
                request.UserId, request.GroupId);

            var result = await _subscriptionService.CreateSubscriptionAsync(
                request.UserId,
                request.GroupId);

            if (result)
            {
                _logger.LogInformation("Successfully created subscription for user {UserId} to group {GroupId}", 
                    request.UserId, request.GroupId);
            }
            else
            {
                _logger.LogWarning("Failed to create subscription for user {UserId} to group {GroupId}", 
                    request.UserId, request.GroupId);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating subscription for user {UserId} to group {GroupId}", 
                request.UserId, request.GroupId);
            throw;
        }
    }
} 