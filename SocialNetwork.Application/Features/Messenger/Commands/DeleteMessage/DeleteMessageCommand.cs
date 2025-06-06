using Elomoas.Application.Interfaces.Services;
using MediatR;

namespace Elomoas.Application.Features.Messenger.Commands.DeleteMessage;

public record DeleteMessageCommand(int Id) : IRequest;

public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand>
{
    private readonly IChatService _chatService;

    public DeleteMessageCommandHandler(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        await _chatService.DeleteMessageAsync(request.Id);
    }
} 