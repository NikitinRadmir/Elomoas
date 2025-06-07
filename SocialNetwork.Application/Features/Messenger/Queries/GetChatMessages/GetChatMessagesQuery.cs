using System.Collections.Generic;
using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Messenger.Queries.GetChatMessages;

public record GetChatMessagesQuery(int ChatId) : IRequest<IEnumerable<Message>>; 