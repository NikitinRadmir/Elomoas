using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Domain.Entities;
using Elomoas.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Elomoas.Application.Features.Courses.Query.GetSubscribedCourses
{
    public class GetSubscribedCoursesQueryHandler : IRequestHandler<GetSubscribedCoursesQuery, IEnumerable<Course>>
    {
        private readonly IGenericRepository<CourseSubscription> _subscriptionRepository;
        private readonly IGenericRepository<Course> _courseRepository;

        public GetSubscribedCoursesQueryHandler(
            IGenericRepository<CourseSubscription> subscriptionRepository,
            IGenericRepository<Course> courseRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<Course>> Handle(GetSubscribedCoursesQuery request, CancellationToken cancellationToken)
        {
            var subscribedCourseIds = await _subscriptionRepository.Entities
                .Where(s => s.UserId == request.UserId)
                .Select(s => s.CourseId)
                .ToListAsync(cancellationToken);

            var subscribedCourses = await _courseRepository.Entities
                .Where(c => subscribedCourseIds.Contains(c.Id))
                .ToListAsync(cancellationToken);

            return subscribedCourses;
        }
    }
}