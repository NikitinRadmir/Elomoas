using Elomoas.Application.Features.Courses.Commands;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using Elomoas.Domain.Entities.Enum;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Elomoas.Application.Features.Groups.Commands;

public record UpdateGroupCommand : IRequest<bool>
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string Img { get; init; }
    public ProgramLanguage PL { get; init; }
}

public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, bool>
{
    private readonly IGroupService _groupService;
    private readonly ILogger<UpdateGroupCommandHandler> _logger;

    public UpdateGroupCommandHandler(IGroupService groupService, ILogger<UpdateGroupCommandHandler> logger)
    {
        _groupService = groupService;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting update for group {Id}", request.Id);

            var existingGroup = await _groupService.GetGroupEntityByIdAsync(request.Id);
            if (existingGroup == null)
            {
                _logger.LogWarning("Group {Id} not found", request.Id);
                return false;
            }

            existingGroup.Name = request.Name ?? existingGroup.Name;
            existingGroup.Description = request.Description ?? existingGroup.Description;
            existingGroup.Img = request.Img ?? existingGroup.Img;
            existingGroup.PL = request.PL;

            var success = await _groupService.UpdateGroupAsync(existingGroup);

            if (success)
            {
                _logger.LogInformation("Successfully updated group {Id}", request.Id);
            }
            else
            {
                _logger.LogWarning("Failed to update group {Id}", request.Id);
            }

            return success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating group {Id}", request.Id);
            throw;
        }
    }
} 