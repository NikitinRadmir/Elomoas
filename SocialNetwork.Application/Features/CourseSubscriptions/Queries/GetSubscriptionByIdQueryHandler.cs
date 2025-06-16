using Elomoas.Application.Features.CourseSubscriptions.Dtos;
using Elomoas.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Elomoas.Application.Features.CourseSubscriptions.Queries;

public class GetSubscriptionByIdQueryHandler : IRequestHandler<GetSubscriptionByIdQuery, CourseSubscriptionDto>
{
    private readonly ICourseSubscriptionService _subscriptionService;
    private readonly ILogger<GetSubscriptionByIdQueryHandler> _logger;

    public GetSubscriptionByIdQueryHandler(
        ICourseSubscriptionService subscriptionService,
        ILogger<GetSubscriptionByIdQueryHandler> logger)
    {
        _subscriptionService = subscriptionService;
        _logger = logger;
    }

    public async Task<CourseSubscriptionDto> Handle(GetSubscriptionByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting subscription with ID: {Id}", request.Id);

        var subscription = await _subscriptionService.GetSubscriptionByIdAsync(request.Id);
        if (subscription == null)
        {
            _logger.LogWarning("Subscription with ID {Id} not found", request.Id);
            return null;
        }

        var result = new CourseSubscriptionDto
        {
            Id = subscription.Id,
            UserId = subscription.UserId,
            CourseId = subscription.CourseId,
            ExpirationDate = subscription.ExpirationDate,
            SubscriptionPrice = subscription.SubscriptionPrice,
            DurationInMonths = subscription.DurationInMonths
        };

        _logger.LogInformation("Successfully retrieved subscription with ID: {Id}", request.Id);
        return result;
    }
} 