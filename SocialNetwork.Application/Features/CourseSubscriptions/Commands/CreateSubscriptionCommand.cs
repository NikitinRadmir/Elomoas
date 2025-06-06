using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;
using System;

namespace Elomoas.Application.Features.CourseSubscriptions.Commands;

public record CreateSubscriptionCommand : IRequest<CourseSubscription>
{
    public int UserId { get; init; }
    public int CourseId { get; init; }
    public decimal SubscriptionPrice { get; init; }
    public int DurationInMonths { get; init; }
    public DateTime ExpirationDate { get; init; }
}

public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, CourseSubscription>
{
    private readonly ICourseSubscriptionService _subscriptionService;

    public CreateSubscriptionCommandHandler(ICourseSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<CourseSubscription> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var subscription = new CourseSubscription
        {
            UserId = request.UserId,
            CourseId = request.CourseId,
            SubscriptionPrice = request.SubscriptionPrice,
            DurationInMonths = request.DurationInMonths,
            ExpirationDate = request.ExpirationDate
        };

        return await _subscriptionService.CreateSubscriptionAsync(subscription);
    }
} 