using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Messenger.Commands.AddMessage;

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