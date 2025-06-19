using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using MediatR;
using Elomoas.Application.Features.Messenger.Queries.Dtos;

namespace Elomoas.Application.Features.Messenger.Commands.UpdateMessage;

public class UpdateMessageCommandHandler : IRequestHandler<UpdateMessageCommand, MessageDto>
{
    private readonly IChatService _chatService;

    public UpdateMessageCommandHandler(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task<MessageDto> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await _chatService.GetMessageByIdAsync(request.Id);
        if (message == null) return null;
        message.Content = request.Content;
        message.IsRead = request.IsRead;
        var updated = await _chatService.UpdateMessageAsync(message);
        return new MessageDto
        {
            Id = updated.Id,
            ChatId = updated.ChatId,
            SenderId = updated.SenderId,
            Content = updated.Content,
            IsRead = updated.IsRead,
            CreatedDate = updated.CreatedDate ?? DateTime.UtcNow
        };
    }
} 