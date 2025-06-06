using MediatR;
using Elomoas.Application.Interfaces.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Elomoas.Application.Features.Courses.Query.CalculatePrice;

public class CalculatePriceQueryHandler : IRequestHandler<CalculatePriceQuery, decimal>
{
    private readonly ICourseService _courseService;

    public CalculatePriceQueryHandler(ICourseService courseService)
    {
        _courseService = courseService;
    }

    public async Task<decimal> Handle(CalculatePriceQuery request, CancellationToken cancellationToken)
    {
        var course = await _courseService.GetCourseByIdAsync(request.CourseId);
        if (course == null)
            return 0;

        var basePrice = course.Price;
        var discountPercent = request.DurationInMonths switch
        {
            3 => 10,
            6 => 20,
            12 => 30,
            _ => 0
        };

        var discountMultiplier = (100 - discountPercent) / 100m;
        return basePrice * discountMultiplier;
    }
} 