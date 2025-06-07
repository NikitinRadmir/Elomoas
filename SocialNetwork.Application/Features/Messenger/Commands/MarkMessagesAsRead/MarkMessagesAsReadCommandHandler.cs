using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using MediatR;

namespace Elomoas.Application.Features.Messenger.Commands.MarkMessagesAsRead;

public class MarkMessagesAsReadCommandHandler : IRequestHandler<MarkMessagesAsReadCommand>
{
    private readonly IChatService _chatService;

    public MarkMessagesAsReadCommandHandler(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task Handle(MarkMessagesAsReadCommand request, CancellationToken cancellationToken)
    {
        await _chatService.MarkMessagesAsReadAsync(request.ChatId, request.UserId);
    }
} 