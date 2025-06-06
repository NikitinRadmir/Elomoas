using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Messenger.Queries.GetChatById;

public record GetChatByIdQuery(int Id) : IRequest<Chat>;

public class GetChatByIdQueryHandler : IRequestHandler<GetChatByIdQuery, Chat>
{
    private readonly IChatService _chatService;

    public GetChatByIdQueryHandler(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task<Chat> Handle(GetChatByIdQuery request, CancellationToken cancellationToken)
    {
        return await _chatService.GetChatByIdAsync(request.Id);
    }
} 