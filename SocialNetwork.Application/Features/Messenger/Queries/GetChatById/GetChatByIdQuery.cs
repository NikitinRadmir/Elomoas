using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Messenger.Queries.GetChatById;

public record GetChatByIdQuery(int Id) : IRequest<Chat>; 