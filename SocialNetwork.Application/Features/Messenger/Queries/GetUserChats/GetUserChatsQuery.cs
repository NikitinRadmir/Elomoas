using MediatR;
using System.Collections.Generic;
using Elomoas.Domain.Entities;

namespace Elomoas.Application.Features.Messenger.Queries.GetUserChats
{
    public record GetUserChatsQuery(string UserId) : IRequest<IEnumerable<Chat>>;
} 