using Elomoas.Application.Features.Courses.Query;
using Elomoas.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Elomoas.mvc.Models.Courses
{
    public class CourseDetailsVM
    {
        public CourseDto Course { get; set; }
        public bool IsSubscribed { get; set; }
        public int SelectedDuration { get; set; }
        public List<SubscriptionDurationOption> DurationOptions { get; set; }
        public SubscriptionInfo SubscriptionInfo { get; set; }

        public CourseDetailsVM()
        {
            DurationOptions = new List<SubscriptionDurationOption>
            {
                new SubscriptionDurationOption { Months = 1, DiscountPercent = 0 },
                new SubscriptionDurationOption { Months = 3, DiscountPercent = 10 },
                new SubscriptionDurationOption { Months = 6, DiscountPercent = 20 },
                new SubscriptionDurationOption { Months = 12, DiscountPercent = 30 }
            };
        }
    }

    public class SubscriptionInfo
    {
        public int DurationInMonths { get; set; }
        public decimal SubscriptionPrice { get; set; }
        public DateTime ExpirationDate { get; set; }
    }

    public class SubscriptionDurationOption
    {
        public int Months { get; set; }
        public int DiscountPercent { get; set; }
        
        public decimal CalculatePrice(decimal basePrice)
        {
            var discount = basePrice * (DiscountPercent / 100m);
            return (basePrice - discount) * Months;
        }

        public string GetDisplayText()
        {
            return DiscountPercent > 0 
                ? $"{Months} месяцев (-{DiscountPercent}%)"
                : $"{Months} месяц";
        }
    }
} 