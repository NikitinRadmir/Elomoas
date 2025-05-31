using MediatR;
using System.Collections.Generic;

namespace Elomoas.Application.Features.Courses.Query.GetLatestCourses;

public record GetLatestCoursesQuery() : IRequest<IEnumerable<CourseDto>>; 