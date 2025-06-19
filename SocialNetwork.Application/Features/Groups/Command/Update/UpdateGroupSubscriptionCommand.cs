using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Elomoas.Application.Features.Groups.Commands;

public record UpdateGroupSubscriptionCommand : IRequest<bool>
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public int GroupId { get; init; }

    public UpdateGroupSubscriptionCommand(int id, int userId, int groupId)
    {
        Id = id;
        UserId = userId;
        GroupId = groupId;
    }
}

public class UpdateGroupSubscriptionCommandHandler : IRequestHandler<UpdateGroupSubscriptionCommand, bool>
{
    private readonly IGroupSubscriptionService _subscriptionService;
    private readonly ILogger<UpdateGroupSubscriptionCommandHandler> _logger;

    public UpdateGroupSubscriptionCommandHandler(
        IGroupSubscriptionService subscriptionService,
        ILogger<UpdateGroupSubscriptionCommandHandler> logger)
    {
        _subscriptionService = subscriptionService;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateGroupSubscriptionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Updating subscription {Id} for user {UserId} to group {GroupId}", 
                request.Id, request.UserId, request.GroupId);

            var result = await _subscriptionService.UpdateSubscriptionAsync(
                request.Id,
                request.UserId,
                request.GroupId);

            if (result)
            {
                _logger.LogInformation("Successfully updated subscription {Id}", request.Id);
            }
            else
            {
                _logger.LogWarning("Failed to update subscription {Id}", request.Id);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating subscription {Id}", request.Id);
            throw;
        }
    }
} 