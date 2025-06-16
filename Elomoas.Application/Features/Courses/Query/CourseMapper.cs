using Elomoas.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Elomoas.Application.Features.Courses.Query
{
    public static class CourseMapper
    {
        public static CourseDto ToDto(this Course course, bool isCurrentUserSubscribed = false)
        {
            if (course == null) return null;

            return new CourseDto
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Img = course.Img,
                Price = course.Price,
                PL = course.PL,
                Video = course.Video,
                Learn = course.Learn,
                IsCurrentUserSubscribed = isCurrentUserSubscribed,
                SubscriptionInfo = null 
            };
        }

        public static IEnumerable<CourseDto> ToDtos(this IEnumerable<Course> courses, bool isCurrentUserSubscribed = false)
        {
            if (courses == null) return Enumerable.Empty<CourseDto>();

            return courses.Select(c => c.ToDto(isCurrentUserSubscribed));
        }
    }
} 