using Elomoas.Application.Features.AppUsers.Query.GetUserById;
using Elomoas.Application.Features.AppUsers.Query;
using Elomoas.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Elomoas.Application.Features.Courses.Query.GetAllCourses
{
    public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, IEnumerable<CourseDto>>
    {
        private readonly ICourseRepository _courseRepository;

        public GetAllCoursesQueryHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<CourseDto>> Handle(GetAllCoursesQuery query, CancellationToken ct)
        {
            var data = await _courseRepository.GetAllCoursesAsync();
            var result = data.OrderBy(x => x.Id).Select(x => new CourseDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Img = x.Img,
                Price = x.Price,
                PL = x.PL,
                Video = x.Video,
                Learn = x.Learn,

            });

            return result ?? new List<CourseDto>() { };

        }
    }
}
