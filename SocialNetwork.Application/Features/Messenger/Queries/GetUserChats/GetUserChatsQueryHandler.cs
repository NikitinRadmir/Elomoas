using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Domain.Entities;
using Elomoas.Application.Interfaces.Services;

namespace Elomoas.Application.Features.Messenger.Queries.GetUserChats
{
    public class GetUserChatsQueryHandler : IRequestHandler<GetUserChatsQuery, IEnumerable<Chat>>
    {
        private readonly IChatService _chatService;

        public GetUserChatsQueryHandler(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<IEnumerable<Chat>> Handle(GetUserChatsQuery request, CancellationToken cancellationToken)
        {
            return await _chatService.GetUserChatsAsync(request.UserId);
        }
    }
} 