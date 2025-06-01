using Elomoas.Application.Features.Courses.Query;
using Elomoas.Domain.Entities;

namespace Elomoas.mvc.Models.Courses
{
    public class CourseDetailsVM
    {
        public CourseDto Course { get; set; }
        public bool IsSubscribed { get; set; }
    }
} 