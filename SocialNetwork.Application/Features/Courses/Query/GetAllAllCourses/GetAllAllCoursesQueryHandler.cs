using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MediatR;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Application.Features.Courses;
using Elomoas.Application.Features.Courses.Query;

namespace SocialNetwork.Application.Features.Courses.Query.GetAllAllCourses
{
    public class GetAllAllCoursesQueryHandler : IRequestHandler<GetAllAllCoursesQuery, IEnumerable<CourseDto>>
    {
        private readonly ICourseRepository _courseRepository;

        public GetAllAllCoursesQueryHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<CourseDto>> Handle(GetAllAllCoursesQuery query, CancellationToken ct)
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
                IsCurrentUserSubscribed = false
            }).ToList();
            return result;
        }
    }
} 