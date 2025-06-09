using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Messenger.Commands.AddMessage;

public class AddMessageCommand : IRequest<Message>
{
    public int ChatId { get; set; }
    public string SenderId { get; set; }
    public string Content { get; set; }
} 