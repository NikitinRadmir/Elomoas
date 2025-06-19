using MediatR;
using Elomoas.Application.Features.Messenger.Queries.Dtos;

namespace Elomoas.Application.Features.Messenger.Commands.UpdateMessage;

public class UpdateMessageCommand : IRequest<MessageDto>
{
    public int Id { get; set; }
    public string Content { get; set; }
    public bool IsRead { get; set; }
} 