using MediatR;

namespace Elomoas.Application.Features.Courses.Query;

public class CalculateCoursePriceQueryHandler : IRequestHandler<CalculateCoursePriceQuery, decimal>
{
    public Task<decimal> Handle(CalculateCoursePriceQuery request, CancellationToken cancellationToken)
    {
        int discount = request.DurationInMonths switch
        {
            3 => 10,
            6 => 20,
            12 => 30,
            _ => 0
        };
        var price = request.CoursePrice * (1 - discount / 100m);
        return Task.FromResult(price);
    }
} 