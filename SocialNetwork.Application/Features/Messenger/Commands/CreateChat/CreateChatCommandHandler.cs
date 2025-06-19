using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using MediatR;
using Elomoas.Application.Features.Messenger.Queries.Dtos;

namespace Elomoas.Application.Features.Messenger.Commands.CreateChat;

public class CreateChatCommandHandler : IRequestHandler<CreateChatCommand, ChatDto>
{
    private readonly IChatService _chatService;

    public CreateChatCommandHandler(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task<ChatDto> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var chat = new Elomoas.Domain.Entities.Chat
        {
            User1Id = request.User1Id,
            User2Id = request.User2Id,
        };
        var created = await _chatService.CreateChatAsync(chat);
        return new ChatDto
        {
            Id = created.Id,
            User1Id = created.User1Id,
            User2Id = created.User2Id
        };
    }
} 