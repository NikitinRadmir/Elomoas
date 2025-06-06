using MediatR;

namespace Elomoas.Application.Features.Courses.Query;

public record CalculateCoursePriceQuery(decimal CoursePrice, int DurationInMonths) : IRequest<decimal>; 