using MediatR;
using System.Collections.Generic;
using Elomoas.Domain.Entities;

namespace Elomoas.Application.Features.Courses.Query.GetSubscribedCourses
{
    public record GetSubscribedCoursesQuery : IRequest<IEnumerable<Course>>
    {
        public int UserId { get; set; }

        public GetSubscribedCoursesQuery(int userId)
        {
            UserId = userId;
        }
    }
}