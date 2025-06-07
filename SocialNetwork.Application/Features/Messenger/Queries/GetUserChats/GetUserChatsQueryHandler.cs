using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using MediatR;

namespace Elomoas.Application.Features.Messenger.Queries.GetUserChats;

public class GetUserChatsQueryHandler : IRequestHandler<GetUserChatsQuery, IEnumerable<UserChatDto>>
{
    private readonly IChatService _chatService;

    public GetUserChatsQueryHandler(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task<IEnumerable<UserChatDto>> Handle(GetUserChatsQuery request, CancellationToken cancellationToken)
    {
        return await _chatService.GetUserChatsWithDetailsAsync(request.UserId);
    }
} 