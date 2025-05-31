using Elomoas.Application.Features.Courses.Query;
using Elomoas.Application.Features.Groups.Query.GetAll;

namespace Elomoas.mvc.Models.Courses
{
    public class CourseVM
    {
        public IEnumerable<CourseDto> PopularCourses { get; set; }
        public IEnumerable<CourseDto> LatestCourses { get; set; }
    }
}
