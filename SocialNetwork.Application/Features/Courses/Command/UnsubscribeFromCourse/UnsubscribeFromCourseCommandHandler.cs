using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Application.Interfaces.Services;

namespace Elomoas.Application.Features.Courses.Command.UnsubscribeFromCourse
{
    public class UnsubscribeFromCourseCommandHandler : IRequestHandler<UnsubscribeFromCourseCommand, bool>
    {
        private readonly ICourseSubscriptionRepository _subscriptionRepository;
        private readonly ICurrentUserService _currentUserService;

        public UnsubscribeFromCourseCommandHandler(
            ICourseSubscriptionRepository subscriptionRepository,
            ICurrentUserService currentUserService)
        {
            _subscriptionRepository = subscriptionRepository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(UnsubscribeFromCourseCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetCurrentAppUserAsync();
            if (currentUser == null)
                return false;

            await _subscriptionRepository.Unsubscribe(currentUser.Id, request.CourseId);
            return true;
        }
    }
}