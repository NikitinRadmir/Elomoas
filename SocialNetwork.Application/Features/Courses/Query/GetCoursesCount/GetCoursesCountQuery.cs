using MediatR;
using System.Collections.Generic;

namespace Elomoas.Application.Features.Courses.Query.GetCoursesCount
{
    public record GetCoursesCountQuery() : IRequest<Dictionary<string, int>>;
} 