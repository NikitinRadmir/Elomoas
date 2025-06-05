using MediatR;
using Elomoas.Domain.Entities;

namespace SocialNetwork.Application.Features.Courses.Query.GetAllAllCourses
{
    public class GetAllAllCoursesQuery : IRequest<IEnumerable<Course>>
    {
    }
} 