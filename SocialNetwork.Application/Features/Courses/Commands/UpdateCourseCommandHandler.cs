using Elomoas.Domain.Entities;
using Elomoas.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Elomoas.Application.Features.Courses.Commands;

public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, bool>
{
    private readonly ICourseService _courseService;
    private readonly ILogger<UpdateCourseCommandHandler> _logger;

    public UpdateCourseCommandHandler(
        ICourseService courseService,
        ILogger<UpdateCourseCommandHandler> logger)
    {
        _courseService = courseService;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting update for course {Id}", request.Id);

            var existingCourse = await _courseService.GetCourseByIdAsync(request.Id);
            if (existingCourse == null)
            {
                _logger.LogWarning("Course {Id} not found", request.Id);
                return false;
            }

            // Update properties while preserving existing data
            existingCourse.Name = request.Name ?? existingCourse.Name;
            existingCourse.Description = request.Description;
            existingCourse.Img = request.Img;
            existingCourse.Price = request.Price;
            existingCourse.PL = request.PL;
            existingCourse.Video = request.Video;
            existingCourse.Learn = request.Learn;
            existingCourse.UpdatedDate = DateTime.UtcNow;

            var success = await _courseService.UpdateCourseAsync(existingCourse);
            
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