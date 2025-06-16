using Elomoas.Application.Features.AppUsers.Query.GetUserById;
using Elomoas.Application.Features.AppUsers.Query;
using Elomoas.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Elomoas.Application.Interfaces.Services;

namespace Elomoas.Application.Features.Courses.Query.GetAllCourses
{
    public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, IEnumerable<CourseDto>>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseSubscriptionRepository _subscriptionRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetAllCoursesQueryHandler(
            ICourseRepository courseRepository,
            ICourseSubscriptionRepository subscriptionRepository,
            ICurrentUserService currentUserService)
        {
            _courseRepository = courseRepository;
            _subscriptionRepository = subscriptionRepository;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<CourseDto>> Handle(GetAllCoursesQuery query, CancellationToken ct)
        {
            var data = await _courseRepository.GetAllCoursesAsync();
            var currentUser = await _currentUserService.GetCurrentAppUserAsync();
            
            return data.OrderBy(x => x.Id).ToDtos();
        }
    }
} 