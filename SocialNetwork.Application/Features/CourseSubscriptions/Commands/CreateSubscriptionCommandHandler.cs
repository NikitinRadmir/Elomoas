using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Elomoas.Application.Features.CourseSubscriptions.Commands;

public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, bool>
{
    private readonly ICourseSubscriptionService _subscriptionService;
    private readonly ILogger<CreateSubscriptionCommandHandler> _logger;

    public CreateSubscriptionCommandHandler(
        ICourseSubscriptionService subscriptionService,
        ILogger<CreateSubscriptionCommandHandler> logger)
    {
        _subscriptionService = subscriptionService;
        _logger = logger;
    }

    public async Task<bool> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation(
                "Creating subscription for user {UserId} to course {CourseId} for {Duration} months",
                request.UserId, request.CourseId, request.DurationInMonths);

            return await _subscriptionService.CreateSubscriptionAsync(
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
                "Error creating subscription for user {UserId} to course {CourseId}",
                request.UserId, request.CourseId);
            throw;
        }
    }
} 