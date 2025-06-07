using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Messenger.Queries.GetChatMessages;

public class GetChatMessagesQueryHandler : IRequestHandler<GetChatMessagesQuery, IEnumerable<Message>>
{
    private readonly IChatService _chatService;

    public GetChatMessagesQueryHandler(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task<IEnumerable<Message>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
    {
        return await _chatService.GetChatMessagesAsync(request.ChatId);
    }
} 