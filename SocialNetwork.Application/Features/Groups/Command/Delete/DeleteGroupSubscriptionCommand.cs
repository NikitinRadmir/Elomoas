using Elomoas.Application.Interfaces.Services;
using MediatR;

namespace Elomoas.Application.Features.Groups.Commands;

public record DeleteGroupSubscriptionCommand(int Id) : IRequest<bool>;

public class DeleteGroupSubscriptionCommandHandler : IRequestHandler<DeleteGroupSubscriptionCommand, bool>
{
    private readonly IGroupSubscriptionService _subscriptionService;

    public DeleteGroupSubscriptionCommandHandler(IGroupSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<bool> Handle(DeleteGroupSubscriptionCommand request, CancellationToken cancellationToken)
    {
        return await _subscriptionService.DeleteSubscriptionAsync(request.Id);
    }
} 