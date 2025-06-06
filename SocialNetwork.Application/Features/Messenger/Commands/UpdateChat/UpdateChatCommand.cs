using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Messenger.Commands.UpdateChat;

public class UpdateChatCommand : IRequest<Chat>
{
    public int Id { get; set; }
    public string User1Id { get; set; }
    public string User2Id { get; set; }
}

public class UpdateChatCommandHandler : IRequestHandler<UpdateChatCommand, Chat>
{
    private readonly IChatService _chatService;

    public UpdateChatCommandHandler(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task<Chat> Handle(UpdateChatCommand request, CancellationToken cancellationToken)
    {
        var chat = new Chat
        {
            Id = request.Id,
            User1Id = request.User1Id,
            User2Id = request.User2Id,
        };

        return await _chatService.UpdateChatAsync(chat);
    }
} 