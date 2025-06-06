using MediatR;
using Elomoas.Domain.Entities;
using Elomoas.Domain.Entities.Enum;
using Elomoas.Application.Interfaces.Services;

namespace Elomoas.Application.Features.Courses.Command;

public record CreateCourseCommand : IRequest<Course>
{
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public string? Img { get; init; }
    public decimal Price { get; init; }
    public ProgramLanguage PL { get; init; }
    public string? Video { get; init; }
    public string? Learn { get; init; }
}

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Course>
{
    private readonly ICourseService _courseService;

    public CreateCourseCommandHandler(ICourseService courseService)
    {
        _courseService = courseService;
    }

    public async Task<Course> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = new Course
        {
            Name = request.Name,
            Description = request.Description,
            Img = string.IsNullOrEmpty(request.Img) ? "/images/v-1.png" : request.Img,
            Price = request.Price,
            PL = request.PL,
            Video = string.IsNullOrEmpty(request.Video) ? "/images/video4.mp4" : request.Video,
            Learn = request.Learn
        };

        return await _courseService.CreateCourseAsync(course);
    }
}