using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Services;
using Elomoas.Domain.Entities;
using MediatR;

namespace Elomoas.Application.Features.Courses.Commands;

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