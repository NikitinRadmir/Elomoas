using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elomoas.Application.Features.Courses.Query;
using Elomoas.Application.Interfaces.Repositories;
using MediatR;

namespace Elomoas.Application.Features.Courses.Query.GetCourseById;

public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, CourseDto>
{
    private readonly ICourseRepository _courseRepository;

    public GetCourseByIdQueryHandler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<CourseDto> Handle(GetCourseByIdQuery query, CancellationToken ct)
    {
        var data = await _courseRepository.GetCourseById(query.id);
        
        if (data == null)
            return null;

        var result = new CourseDto
        {
            Id = data.Id,
            Name = data.Name,
            Description = data.Description,
            Img = data.Img,
            Price = data.Price,
            PL = data.PL,
            Video = data.Video,
            Learn = data.Learn
        };

        return result;
    }
}
