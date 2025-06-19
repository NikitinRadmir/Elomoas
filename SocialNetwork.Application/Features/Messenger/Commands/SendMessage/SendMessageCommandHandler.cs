using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Application.Features.Messenger.Queries.Dtos;

namespace Elomoas.Application.Features.Messenger.Commands.SendMessage
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, MessageDto>
    {
        private readonly IChatService _chatService;

        public SendMessageCommandHandler(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<MessageDto> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var message = await _chatService.SendMessageAsync(request.SenderId, request.RecipientId, request.Content);
            if (message == null) return null;
            return new MessageDto
            {
                Id = message.Id,
                ChatId = message.ChatId,
                SenderId = message.SenderId,
                Content = message.Content,
                IsRead = message.IsRead,
                CreatedDate = message.CreatedDate ?? DateTime.UtcNow
            };
        }
    }
} 