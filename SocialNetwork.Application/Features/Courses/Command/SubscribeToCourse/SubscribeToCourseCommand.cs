using MediatR;

namespace Elomoas.Application.Features.Courses.Command.SubscribeToCourse
{
    public class SubscribeToCourseCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public int DurationInMonths { get; set; }

        public SubscribeToCourseCommand(int userId, int courseId, int durationInMonths)
        {
            UserId = userId;
            CourseId = courseId;
            DurationInMonths = durationInMonths;
        }
    }
}