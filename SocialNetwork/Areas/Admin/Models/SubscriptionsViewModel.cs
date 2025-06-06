using Elomoas.Application.Features.Courses.Query;
using Elomoas.Domain.Entities;
using System;

namespace SocialNetwork.Areas.Admin.Models;

public class SubscriptionsViewModel
{
    public IEnumerable<CourseSubscription> Subscriptions { get; set; }
}

public class CreateSubscriptionViewModel
{
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public decimal SubscriptionPrice { get; set; }
    public int DurationInMonths { get; set; }
    public DateTime ExpirationDate { get; set; }
}

public class UpdateSubscriptionViewModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public decimal SubscriptionPrice { get; set; }
    public int DurationInMonths { get; set; }
    public DateTime ExpirationDate { get; set; }
} 