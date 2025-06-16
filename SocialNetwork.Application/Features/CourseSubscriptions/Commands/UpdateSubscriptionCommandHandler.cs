using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Elomoas.Application.Features.CourseSubscriptions.Commands;

public class UpdateSubscriptionCommandHandler : IRequestHandler<UpdateSubscriptionCommand, bool>
{
    private readonly ICourseSubscriptionService _subscriptionService;
    private readonly ILogger<UpdateSubscriptionCommandHandler> _logger;

    public UpdateSubscriptionCommandHandler(
        ICourseSubscriptionService subscriptionService,
        ILogger<UpdateSubscriptionCommandHandler> logger)
    {
        _subscriptionService = subscriptionService;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation(
                "Updating subscription {Id} for user {UserId} to course {CourseId}",
                request.Id, request.UserId, request.CourseId);

            return await _subscriptionService.UpdateSubscriptionAsync(
                request.Id,
                request.UserId,
                request.CourseId,
                request.SubscriptionPrice,
                request.DurationInMonths,
                request.ExpirationDate
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "Error updating subscription {Id} for user {UserId} to course {CourseId}",
                request.Id, request.UserId, request.CourseId);
            throw;
        }
    }
} 