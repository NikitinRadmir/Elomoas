using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Messenger.Commands.UpdateChat;

public class UpdateChatCommand : IRequest<Chat>
{
    public int Id { get; set; }
    public string User1Id { get; set; }
    public string User2Id { get; set; }
} 