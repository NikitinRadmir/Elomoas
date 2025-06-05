using MediatR;
using Elomoas.Domain.Entities;
using Elomoas.Application.Interfaces.Repositories;

namespace SocialNetwork.Application.Features.Courses.Query.GetAllAllCourses
{
    public class GetAllAllCoursesQueryHandler : IRequestHandler<GetAllAllCoursesQuery, IEnumerable<Course>>
    {
        private readonly ICourseRepository _courseRepository;

        public GetAllAllCoursesQueryHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<Course>> Handle(GetAllAllCoursesQuery request, CancellationToken cancellationToken)
        {
            var courses = await _courseRepository.GetAllCoursesAsync();
            return courses;
        }
    }
} 