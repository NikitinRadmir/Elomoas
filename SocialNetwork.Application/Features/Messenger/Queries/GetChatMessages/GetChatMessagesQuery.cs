using System.Collections.Generic;
using MediatR;
using Elomoas.Application.Features.Messenger.Queries.Dtos;

namespace Elomoas.Application.Features.Messenger.Queries.GetChatMessages;

public record GetChatMessagesQuery(int ChatId) : IRequest<IEnumerable<MessageDto>>; 