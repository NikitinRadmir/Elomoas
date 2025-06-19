using MediatR;
using Elomoas.Application.Features.Messenger.Queries.Dtos;

namespace Elomoas.Application.Features.Messenger.Commands.AddMessage;

public class AddMessageCommand : IRequest<MessageDto>
{
    public int ChatId { get; set; }
    public string SenderId { get; set; }
    public string Content { get; set; }
} 