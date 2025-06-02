using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elomoas.Application.Interfaces.Repositories;

namespace Elomoas.Application.Features.Courses.Query.GetCoursesCount
{
    public class GetCoursesCountQueryHandler : IRequestHandler<GetCoursesCountQuery, Dictionary<string, int>>
    {
        private readonly ICourseRepository _courseRepository;

        public GetCoursesCountQueryHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<Dictionary<string, int>> Handle(GetCoursesCountQuery request, CancellationToken cancellationToken)
        {
            return await _courseRepository.GetCoursesCountByPL();
        }
    }
} 