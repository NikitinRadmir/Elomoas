using MediatR;

namespace Elomoas.Application.Features.Courses.Command.SubscribeToCourse
{
    public record SubscribeToCourseCommand : IRequest<bool>
    {
        public int CourseId { get; set; }
        public int DurationInMonths { get; set; }

        public SubscribeToCourseCommand(int courseId, int durationInMonths)
        {
            CourseId = courseId;
            DurationInMonths = durationInMonths;
        }
    }
}