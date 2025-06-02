using Elomoas.Application.Features.Courses.Query;
using Elomoas.Application.Features.Groups.Query.GetAll;
using System.Collections.Generic;

namespace Elomoas.mvc.Models.Courses
{
    public class CourseVM
    {
        public IEnumerable<CourseDto> PopularCourses { get; set; }
        public IEnumerable<CourseDto> LatestCourses { get; set; }
        public Dictionary<string, int> CoursesCount { get; set; }
        public string SearchTerm { get; set; }
        public string SelectedCategory { get; set; }
    }
}
