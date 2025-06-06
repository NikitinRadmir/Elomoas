using System;

namespace Elomoas.Application.Features.Courses.Dto;

public class SubscriptionInfoDto
{
    public int DurationInMonths { get; set; }
    public decimal SubscriptionPrice { get; set; }
    public DateTime ExpirationDate { get; set; }
} 