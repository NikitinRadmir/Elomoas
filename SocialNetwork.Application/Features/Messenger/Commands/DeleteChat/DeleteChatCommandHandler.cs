using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using MediatR;

namespace Elomoas.Application.Features.Messenger.Commands.DeleteChat;

public class DeleteChatCommandHandler : IRequestHandler<DeleteChatCommand>
{
    private readonly IChatService _chatService;

    public DeleteChatCommandHandler(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task Handle(DeleteChatCommand request, CancellationToken cancellationToken)
    {
        await _chatService.DeleteChatAsync(request.Id);
    }
} 