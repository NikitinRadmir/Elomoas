using MediatR;

namespace Elomoas.Application.Features.Courses.Command.UnsubscribeFromCourse
{
    public class UnsubscribeFromCourseCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }

        public UnsubscribeFromCourseCommand(int userId, int courseId)
        {
            UserId = userId;
            CourseId = courseId;
        }
    }
}