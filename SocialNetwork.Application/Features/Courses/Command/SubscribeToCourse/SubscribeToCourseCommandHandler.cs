using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Application.Interfaces.Services;

namespace Elomoas.Application.Features.Courses.Command.SubscribeToCourse
{
    public class SubscribeToCourseCommandHandler : IRequestHandler<SubscribeToCourseCommand, bool>
    {
        private readonly ICourseSubscriptionRepository _subscriptionRepository;
        private readonly ICurrentUserService _currentUserService;

        public SubscribeToCourseCommandHandler(
            ICourseSubscriptionRepository subscriptionRepository,
            ICurrentUserService currentUserService)
        {
            _subscriptionRepository = subscriptionRepository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(SubscribeToCourseCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetCurrentAppUserAsync();
            if (currentUser == null)
                return false;

            await _subscriptionRepository.Subscribe(currentUser.Id, request.CourseId, request.DurationInMonths);
            return true;
        }
    }
}