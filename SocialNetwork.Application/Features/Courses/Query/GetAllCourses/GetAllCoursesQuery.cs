using Elomoas.Application.Features.AppUsers.Query;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elomoas.Application.Features.Courses.Query.GetAllCourses
{
    public record GetAllCoursesQuery() : IRequest<IEnumerable<CourseDto>>;
}
