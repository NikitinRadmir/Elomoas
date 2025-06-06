using MediatR;
using System.Collections.Generic;
using Elomoas.Application.Features.Messenger.Dto;

namespace Elomoas.Application.Features.Messenger.Queries.GetUserChats;

public record GetUserChatsQuery(string UserId) : IRequest<List<UserChatDto>>; 