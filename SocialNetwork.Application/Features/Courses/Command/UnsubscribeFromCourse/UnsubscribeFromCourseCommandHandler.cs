using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Repositories;

namespace Elomoas.Application.Features.Courses.Command.UnsubscribeFromCourse
{
    public class UnsubscribeFromCourseCommandHandler : IRequestHandler<UnsubscribeFromCourseCommand, bool>
    {
        private readonly ICourseSubscriptionRepository _subscriptionRepository;

        public UnsubscribeFromCourseCommandHandler(ICourseSubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<bool> Handle(UnsubscribeFromCourseCommand request, CancellationToken cancellationToken)
        {
            await _subscriptionRepository.Unsubscribe(request.UserId, request.CourseId);
            return true;
        }
    }
}