using MediatR;
using Elomoas.Application.Features.Messenger.Queries.Dtos;

namespace Elomoas.Application.Features.Messenger.Commands.CreateChat;

public class CreateChatCommand : IRequest<ChatDto>
{
    public string User1Id { get; set; }
    public string User2Id { get; set; }
} 