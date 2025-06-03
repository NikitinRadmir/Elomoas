using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Repositories;

namespace Elomoas.Application.Features.Courses.Command.SubscribeToCourse
{
    public class SubscribeToCourseCommandHandler : IRequestHandler<SubscribeToCourseCommand, bool>
    {
        private readonly ICourseSubscriptionRepository _subscriptionRepository;

        public SubscribeToCourseCommandHandler(ICourseSubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<bool> Handle(SubscribeToCourseCommand request, CancellationToken cancellationToken)
        {
            await _subscriptionRepository.Subscribe(request.UserId, request.CourseId, request.DurationInMonths);
            return true;
        }
    }
}