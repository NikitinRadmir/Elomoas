using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Messenger.Queries.GetMessageById;

public record GetMessageByIdQuery(int Id) : IRequest<Message>; 