using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Messenger.Queries.GetMessageById;

public record GetMessageByIdQuery(int Id) : IRequest<Message>;

public class GetMessageByIdQueryHandler : IRequestHandler<GetMessageByIdQuery, Message>
{
    private readonly IChatService _chatService;

    public GetMessageByIdQueryHandler(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task<Message> Handle(GetMessageByIdQuery request, CancellationToken cancellationToken)
    {
        return await _chatService.GetMessageByIdAsync(request.Id);
    }
} 