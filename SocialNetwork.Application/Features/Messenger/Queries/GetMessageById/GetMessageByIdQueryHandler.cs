using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using MediatR;
using Elomoas.Application.Features.Messenger.Queries.Dtos;

namespace Elomoas.Application.Features.Messenger.Queries.GetMessageById;

public class GetMessageByIdQueryHandler : IRequestHandler<GetMessageByIdQuery, MessageDto>
{
    private readonly IChatService _chatService;

    public GetMessageByIdQueryHandler(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task<MessageDto> Handle(GetMessageByIdQuery request, CancellationToken cancellationToken)
    {
        var message = await _chatService.GetMessageByIdAsync(request.Id);
        if (message == null) return null;
        return new MessageDto
        {
            Id = message.Id,
            ChatId = message.ChatId,
            SenderId = message.SenderId,
            Content = message.Content,
            IsRead = message.IsRead,
            CreatedDate = message.CreatedDate ?? DateTime.UtcNow
        };
    }
} 