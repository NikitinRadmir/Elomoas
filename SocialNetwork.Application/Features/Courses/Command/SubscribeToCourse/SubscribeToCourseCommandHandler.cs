using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Elomoas.Application.Features.Courses.Command.SubscribeToCourse
{
    public class SubscribeToCourseCommandHandler : IRequestHandler<SubscribeToCourseCommand, bool>
    {
        private readonly ICourseSubscriptionRepository _subscriptionRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<SubscribeToCourseCommandHandler> _logger;

        public SubscribeToCourseCommandHandler(
            ICourseSubscriptionRepository subscriptionRepository,
            ICurrentUserService currentUserService,
            ILogger<SubscribeToCourseCommandHandler> logger)
        {
            _subscriptionRepository = subscriptionRepository;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<bool> Handle(SubscribeToCourseCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Processing subscription command for course {CourseId} with duration {Duration} months",
                request.CourseId,
                request.DurationInMonths);

            var currentUser = await _currentUserService.GetCurrentAppUserAsync();
            if (currentUser == null)
            {
                _logger.LogWarning("Current user not found");
                return false;
            }

            _logger.LogInformation("Current user ID: {UserId}", currentUser.Id);

            try
            {
                await _subscriptionRepository.Subscribe(currentUser.Id, request.CourseId, request.DurationInMonths);
                _logger.LogInformation("Successfully subscribed user {UserId} to course {CourseId}", currentUser.Id, request.CourseId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error subscribing user {UserId} to course {CourseId}", currentUser.Id, request.CourseId);
                throw;
            }
        }
    }
}