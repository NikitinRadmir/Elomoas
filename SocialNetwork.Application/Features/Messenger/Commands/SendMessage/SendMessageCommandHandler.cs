using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Domain.Entities;
using Elomoas.Application.Interfaces.Services;

namespace Elomoas.Application.Features.Messenger.Commands.SendMessage
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Message>
    {
        private readonly IChatService _chatService;

        public SendMessageCommandHandler(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<Message> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            return await _chatService.SendMessageAsync(request.SenderId, request.RecipientId, request.Content);
        }
    }
} 