using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using MediatR;
using Elomoas.Application.Features.Messenger.Queries.Dtos;

namespace Elomoas.Application.Features.Messenger.Queries.GetOrCreateChat;

public class GetOrCreateChatQueryHandler : IRequestHandler<GetOrCreateChatQuery, ChatDto>
{
    private readonly IChatService _chatService;

    public GetOrCreateChatQueryHandler(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task<ChatDto> Handle(GetOrCreateChatQuery request, CancellationToken cancellationToken)
    {
        var chat = await _chatService.GetOrCreateChatAsync(request.UserId, request.FriendId);
        if (chat == null) return null;
        return new ChatDto
        {
            Id = chat.Id,
            User1Id = chat.User1Id,
            User2Id = chat.User2Id
        };
    }
} 