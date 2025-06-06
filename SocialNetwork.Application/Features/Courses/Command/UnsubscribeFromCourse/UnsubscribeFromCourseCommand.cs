using MediatR;

namespace Elomoas.Application.Features.Courses.Command.UnsubscribeFromCourse
{
    public class UnsubscribeFromCourseCommand : IRequest<bool>
    {
        public int CourseId { get; set; }

        public UnsubscribeFromCourseCommand(int courseId)
        {
            CourseId = courseId;
        }
    }
}