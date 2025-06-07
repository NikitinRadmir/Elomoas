using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Messenger.Queries.GetOrCreateChat;

public class GetOrCreateChatQueryHandler : IRequestHandler<GetOrCreateChatQuery, Chat>
{
    private readonly IChatService _chatService;

    public GetOrCreateChatQueryHandler(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task<Chat> Handle(GetOrCreateChatQuery request, CancellationToken cancellationToken)
    {
        return await _chatService.GetOrCreateChatAsync(request.UserId, request.FriendId);
    }
} 