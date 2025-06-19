using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using MediatR;
using Elomoas.Application.Features.Messenger.Queries.Dtos;

namespace Elomoas.Application.Features.Messenger.Commands.UpdateChat;

public class UpdateChatCommandHandler : IRequestHandler<UpdateChatCommand, ChatDto>
{
    private readonly IChatService _chatService;

    public UpdateChatCommandHandler(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task<ChatDto> Handle(UpdateChatCommand request, CancellationToken cancellationToken)
    {
        var chat = new Elomoas.Domain.Entities.Chat
        {
            Id = request.Id,
            User1Id = request.User1Id,
            User2Id = request.User2Id,
        };
        var updated = await _chatService.UpdateChatAsync(chat);
        return new ChatDto
        {
            Id = updated.Id,
            User1Id = updated.User1Id,
            User2Id = updated.User2Id
        };
    }
} 