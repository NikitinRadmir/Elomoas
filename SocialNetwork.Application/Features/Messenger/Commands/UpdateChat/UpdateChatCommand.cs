using MediatR;
using Elomoas.Application.Features.Messenger.Queries.Dtos;

namespace Elomoas.Application.Features.Messenger.Commands.UpdateChat;

public class UpdateChatCommand : IRequest<ChatDto>
{
    public int Id { get; set; }
    public string User1Id { get; set; }
    public string User2Id { get; set; }
} 