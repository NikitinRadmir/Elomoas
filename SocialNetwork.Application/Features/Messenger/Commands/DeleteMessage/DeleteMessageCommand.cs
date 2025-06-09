using MediatR;

namespace Elomoas.Application.Features.Messenger.Commands.DeleteMessage;

public record DeleteMessageCommand(int Id) : IRequest; 