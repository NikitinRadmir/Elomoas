using MediatR;

namespace Elomoas.Application.Features.Courses.Query.CalculatePrice;

public record CalculatePriceQuery(int CourseId, int DurationInMonths) : IRequest<decimal>; 