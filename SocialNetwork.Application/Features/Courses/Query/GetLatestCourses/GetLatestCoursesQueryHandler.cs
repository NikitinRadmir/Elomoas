using Elomoas.Application.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elomoas.Application.Features.Courses.Query.GetLatestCourses;

public class GetLatestCoursesQueryHandler : IRequestHandler<GetLatestCoursesQuery, IEnumerable<CourseDto>>
{
    private readonly ICourseRepository _courseRepository;

    public GetLatestCoursesQueryHandler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<IEnumerable<CourseDto>> Handle(GetLatestCoursesQuery query, CancellationToken ct)
    {
        var data = await _courseRepository.GetAllCoursesAsync();
        var result = data.OrderByDescending(x => x.Id).Take(7).Select(x => new CourseDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Img = x.Img,
            Price = x.Price,
            PL = x.PL,
            Video = x.Video,
            Learn = x.Learn
        });

        return result;
    }
} 