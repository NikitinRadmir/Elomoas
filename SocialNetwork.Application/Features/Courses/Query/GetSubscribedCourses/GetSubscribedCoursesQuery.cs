using MediatR;
using System.Collections.Generic;
using Elomoas.Domain.Entities;

namespace Elomoas.Application.Features.Courses.Query.GetSubscribedCourses
{
    public record GetSubscribedCoursesQuery(int UserId) : IRequest<IEnumerable<CourseDto>>
    {

    }
}