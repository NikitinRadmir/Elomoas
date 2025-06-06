using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Messenger.Commands.AddMessage;

public class AddMessageCommand : IRequest<Message>
{
    public int ChatId { get; set; }
    public string SenderId { get; set; }
    public string Content { get; set; }
}

public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand, Message>
{
    private readonly IChatService _chatService;

    public AddMessageCommandHandler(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task<Message> Handle(AddMessageCommand request, CancellationToken cancellationToken)
    {
        return await _chatService.AddMessageAsync(request.ChatId, request.SenderId, request.Content);
    }
} 