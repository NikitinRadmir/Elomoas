using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Messenger.Commands.UpdateMessage;

public class UpdateMessageCommand : IRequest<Message>
{
    public int Id { get; set; }
    public string Content { get; set; }
    public bool IsRead { get; set; }
} 