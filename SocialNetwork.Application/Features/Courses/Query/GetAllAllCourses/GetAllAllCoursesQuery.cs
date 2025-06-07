using MediatR;
using Elomoas.Domain.Entities;

namespace SocialNetwork.Application.Features.Courses.Query.GetAllAllCourses
{
    public record GetAllAllCoursesQuery : IRequest<IEnumerable<Course>>
    {
    }
} 