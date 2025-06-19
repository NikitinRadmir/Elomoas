using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using MediatR;
using Elomoas.Application.Features.Messenger.Queries.Dtos;

namespace Elomoas.Application.Features.Messenger.Commands.AddMessage;

public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand, MessageDto>
{
    private readonly IChatService _chatService;

    public AddMessageCommandHandler(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task<MessageDto> Handle(AddMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await _chatService.AddMessageAsync(request.ChatId, request.SenderId, request.Content);
        if (message == null) return null;
        return new MessageDto
        {
            Id = message.Id,
            ChatId = message.ChatId,
            SenderId = message.SenderId,
            Content = message.Content,
            IsRead = message.IsRead
        };
    }
} 