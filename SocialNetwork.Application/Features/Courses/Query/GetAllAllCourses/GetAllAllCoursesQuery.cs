using System.Collections.Generic;
using MediatR;
using Elomoas.Application.Features.Courses;
using Elomoas.Application.Features.Courses.Query;

namespace SocialNetwork.Application.Features.Courses.Query.GetAllAllCourses
{
    public record GetAllAllCoursesQuery : IRequest<IEnumerable<CourseDto>>
    {
    }
} 