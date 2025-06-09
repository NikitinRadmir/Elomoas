using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.CourseSubscriptions.Commands;

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