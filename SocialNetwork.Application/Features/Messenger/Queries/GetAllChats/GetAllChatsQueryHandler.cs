using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Messenger.Queries.GetAllChats;

public class GetAllChatsQueryHandler : IRequestHandler<GetAllChatsQuery, IEnumerable<Chat>>
{
    private readonly IChatService _chatService;

    public GetAllChatsQueryHandler(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task<IEnumerable<Chat>> Handle(GetAllChatsQuery request, CancellationToken cancellationToken)
    {
        return await _chatService.GetAllChatsAsync();
    }
} 