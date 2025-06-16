using System;

namespace Elomoas.Application.Features.CourseSubscriptions.Dtos;

public class CourseSubscriptionDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public decimal SubscriptionPrice { get; set; }
    public int DurationInMonths { get; set; }
    public DateTime ExpirationDate { get; set; }
} 