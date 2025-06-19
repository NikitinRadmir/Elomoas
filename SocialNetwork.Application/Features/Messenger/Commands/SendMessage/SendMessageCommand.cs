using MediatR;
using Elomoas.Application.Features.Messenger.Queries.Dtos;

namespace Elomoas.Application.Features.Messenger.Commands.SendMessage;

public record SendMessageCommand(string SenderId, string RecipientId, string Content) : IRequest<MessageDto>; 