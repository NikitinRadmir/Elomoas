using MediatR;

namespace Elomoas.Application.Features.Courses.Query;

public record IsSubscribedToCourseQuery(int AppUserId, int CourseId) : IRequest<bool>; 