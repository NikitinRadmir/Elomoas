using Elomoas.Application.Features.Courses.Command;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using Elomoas.Domain.Entities.Enum;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Elomoas.Application.Features.Groups.Command;

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
    private readonly ILogger<UpdateCourseCommandHandler> _logger;

    public UpdateGroupCommandHandler(IGroupService groupService, ILogger<UpdateCourseCommandHandler> logger)
    {
        _groupService = groupService;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting update for course {Id}", request.Id);

            var existingCourse = await _groupService.GetGroupByIdAsync(request.Id);
            if (existingCourse == null)
            {
                _logger.LogWarning("Course {Id} not found", request.Id);
                return false;
            }

            // Update properties while preserving existing data
            existingCourse.Name = request.Name ?? existingCourse.Name;
            existingCourse.Description = request.Description;
            existingCourse.Img = request.Img;

            existingCourse.PL = request.PL;


            var success = await _groupService.UpdateGroupAsync(existingCourse);

            if (success)
            {
                _logger.LogInformation("Successfully updated course {Id}", request.Id);
            }
            else
            {
                _logger.LogWarning("Failed to update course {Id}", request.Id);
            }

            return success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating course {Id}", request.Id);
            throw;
        }
    }
}