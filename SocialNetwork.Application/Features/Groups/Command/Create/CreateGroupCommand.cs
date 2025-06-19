using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using Elomoas.Domain.Entities.Enum;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Elomoas.Application.Features.Groups.Commands;

public record CreateGroupCommand : IRequest<bool>
{
    public string Name { get; init; }
    public string Description { get; init; }
    public string Img { get; init; }
    public ProgramLanguage PL { get; init; }

    public CreateGroupCommand(string name, string description, string img, ProgramLanguage pl)
    {
        Name = name;
        Description = description;
        Img = img;
        PL = pl;
    }
}

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, bool>
{
    private readonly IGroupService _groupService;
    private readonly ILogger<CreateGroupCommandHandler> _logger;

    public CreateGroupCommandHandler(
        IGroupService groupService,
        ILogger<CreateGroupCommandHandler> logger)
    {
        _groupService = groupService;
        _logger = logger;
    }

    public async Task<bool> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Creating new group with name {Name}", request.Name);

            var result = await _groupService.CreateGroupAsync(
                request.Name,
                request.Description,
                request.Img,
                request.PL);

            if (result)
            {
                _logger.LogInformation("Successfully created group with name {Name}", request.Name);
            }
            else
            {
                _logger.LogWarning("Failed to create group with name {Name}", request.Name);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating group with name {Name}", request.Name);
            throw;
        }
    }
} 