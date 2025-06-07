using MediatR;

namespace Elomoas.Application.Features.Messenger.Commands.MarkMessagesAsRead;

public record MarkMessagesAsReadCommand(int ChatId, string UserId) : IRequest; 