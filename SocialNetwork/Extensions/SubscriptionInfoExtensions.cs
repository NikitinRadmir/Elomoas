using Elomoas.Application.Features.Courses.Query;
using Elomoas.mvc.Models.Courses;

namespace Elomoas.Extensions
{
    public static class SubscriptionInfoExtensions
    {
        public static SubscriptionInfo ToViewModel(this SubscriptionInfoDto dto)
        {
            if (dto == null)
                return null;

            return new SubscriptionInfo
            {
                DurationInMonths = dto.DurationInMonths,
                SubscriptionPrice = dto.SubscriptionPrice,
                ExpirationDate = dto.ExpirationDate
            };
        }

        public static SubscriptionInfoDto ToDto(this SubscriptionInfo model)
        {
            if (model == null)
                return null;

            return new SubscriptionInfoDto
            {
                DurationInMonths = model.DurationInMonths,
                SubscriptionPrice = model.SubscriptionPrice,
                ExpirationDate = model.ExpirationDate
            };
        }
    }
} 