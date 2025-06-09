using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Messenger.Commands.CreateChat;

public class CreateChatCommandHandler : IRequestHandler<CreateChatCommand, Chat>
{
    private readonly IChatService _chatService;

    public CreateChatCommandHandler(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task<Chat> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var chat = new Chat
        {
            User1Id = request.User1Id,
            User2Id = request.User2Id,
        };

        return await _chatService.CreateChatAsync(chat);
    }
} 