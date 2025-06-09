using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using MediatR;

namespace Elomoas.Application.Features.CourseSubscriptions.Commands;

public class DeleteSubscriptionCommandHandler : IRequestHandler<DeleteSubscriptionCommand, bool>
{
    private readonly ICourseSubscriptionService _subscriptionService;

    public DeleteSubscriptionCommandHandler(ICourseSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<bool> Handle(DeleteSubscriptionCommand request, CancellationToken cancellationToken)
    {
        return await _subscriptionService.DeleteSubscriptionAsync(request.Id);
    }
} 