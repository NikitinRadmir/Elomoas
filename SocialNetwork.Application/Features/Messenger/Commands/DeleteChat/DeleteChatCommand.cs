using MediatR;

namespace Elomoas.Application.Features.Messenger.Commands.DeleteChat;

public record DeleteChatCommand(int Id) : IRequest; 