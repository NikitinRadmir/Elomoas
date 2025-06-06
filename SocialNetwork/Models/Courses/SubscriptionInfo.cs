using System;

namespace Elomoas.mvc.Models.Courses
{
    public class SubscriptionInfo
    {
        public int DurationInMonths { get; set; }
        public decimal SubscriptionPrice { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
} 