using Elomoas.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Elomoas.Application.Features.Messenger.Queries.GetAllChats;

public record GetAllChatsQuery : IRequest<IEnumerable<Chat>>; 