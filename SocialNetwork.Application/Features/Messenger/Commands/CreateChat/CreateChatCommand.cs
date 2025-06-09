using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Messenger.Commands.CreateChat;

public class CreateChatCommand : IRequest<Chat>
{
    public string User1Id { get; set; }
    public string User2Id { get; set; }
} 