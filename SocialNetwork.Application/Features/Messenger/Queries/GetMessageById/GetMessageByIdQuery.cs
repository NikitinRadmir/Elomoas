using MediatR;
using Elomoas.Application.Features.Messenger.Queries.Dtos;

namespace Elomoas.Application.Features.Messenger.Queries.GetMessageById;

public record GetMessageByIdQuery(int Id) : IRequest<MessageDto>; 