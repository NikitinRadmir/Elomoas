using MediatR;
using Elomoas.Domain.Entities;

namespace Elomoas.Application.Features.Messenger.Commands.SendMessage
{
    public record SendMessageCommand(string SenderId, string RecipientId, string Content) : IRequest<Message>;
} 