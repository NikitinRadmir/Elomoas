using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using Elomoas.Domain.Entities.Enum;
using MediatR;

namespace Elomoas.Application.Features.Groups.Command;

public record CreateGroupCommand : IRequest<Group>
{
    public string Name { get; init; }
    public string Description { get; init; }
    public string Img { get; init; }
    public ProgramLanguage PL { get; init; }
}

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Group>
{
    private readonly IGroupService _groupService;

    public CreateGroupCommandHandler(IGroupService groupService)
    {
        _groupService = groupService;
    }

    public async Task<Group> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var group = new Group
        {
            Name = request.Name,
            Description = request.Description,

        };

        return await _groupService.CreateGroupAsync(group);
    }
}