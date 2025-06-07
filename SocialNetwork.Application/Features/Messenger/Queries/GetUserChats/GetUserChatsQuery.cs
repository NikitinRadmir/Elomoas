using MediatR;
using System.Collections.Generic;

namespace Elomoas.Application.Features.Messenger.Queries.GetUserChats
{
    public record GetUserChatsQuery(string UserId) : IRequest<IEnumerable<UserChatDto>>;
} 