using Elomoas.Application.Interfaces.Services;
using MediatR;

namespace Elomoas.Application.Features.Groups.Commands;

public record DeleteGroupCommand(int Id) : IRequest<bool>;

public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, bool>
{
    private readonly IGroupService _groupService;

    public DeleteGroupCommandHandler(IGroupService groupService)
    {
        _groupService = groupService;
    }

    public async Task<bool> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
    {
        return await _groupService.DeleteGroupAsync(request.Id);
    }
} 