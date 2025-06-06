using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Messenger.Commands.UpdateMessage;

public class UpdateMessageCommand : IRequest<Message>
{
    public int Id { get; set; }
    public string Content { get; set; }
    public bool IsRead { get; set; }
}

public class UpdateMessageCommandHandler : IRequestHandler<UpdateMessageCommand, Message>
{
    private readonly IChatService _chatService;

    public UpdateMessageCommandHandler(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task<Message> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await _chatService.GetMessageByIdAsync(request.Id);
        message.Content = request.Content;
        message.IsRead = request.IsRead;

        return await _chatService.UpdateMessageAsync(message);
    }
} 