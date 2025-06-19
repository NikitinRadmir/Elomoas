using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using MediatR;
using Elomoas.Application.Features.Messenger.Queries.Dtos;
using System.Linq;

namespace Elomoas.Application.Features.Messenger.Queries.GetChatMessages;

public class GetChatMessagesQueryHandler : IRequestHandler<GetChatMessagesQuery, IEnumerable<MessageDto>>
{
    private readonly IChatService _chatService;

    public GetChatMessagesQueryHandler(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task<IEnumerable<MessageDto>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
    {
        var messages = await _chatService.GetChatMessagesAsync(request.ChatId);
        return messages.Select(x => new MessageDto
        {
            Id = x.Id,
            ChatId = x.ChatId,
            SenderId = x.SenderId,
            Content = x.Content,
            IsRead = x.IsRead,
            CreatedDate = x.CreatedDate ?? DateTime.UtcNow
        });
    }
} 