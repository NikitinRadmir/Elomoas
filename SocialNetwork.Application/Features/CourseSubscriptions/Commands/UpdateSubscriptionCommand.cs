using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;
using System;

namespace Elomoas.Application.Features.CourseSubscriptions.Commands;

public record UpdateSubscriptionCommand : IRequest<bool>
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public int CourseId { get; init; }
    public decimal SubscriptionPrice { get; init; }
    public int DurationInMonths { get; init; }
    public DateTime ExpirationDate { get; init; }
}

public class UpdateSubscriptionCommandHandler : IRequestHandler<UpdateSubscriptionCommand, bool>
{
    private readonly ICourseSubscriptionService _subscriptionService;

    public UpdateSubscriptionCommandHandler(ICourseSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<bool> Handle(UpdateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var subscription = new CourseSubscription
        {
            Id = request.Id,
            UserId = request.UserId,
            CourseId = request.CourseId,
            SubscriptionPrice = request.SubscriptionPrice,
            DurationInMonths = request.DurationInMonths,
            ExpirationDate = request.ExpirationDate
        };

        return await _subscriptionService.UpdateSubscriptionAsync(subscription);
    }
} 