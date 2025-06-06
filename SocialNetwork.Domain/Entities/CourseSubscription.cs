using Elomoas.Domain.Common;

namespace Elomoas.Domain.Entities;

public class CourseSubscription : BaseAuditableEntity
{
    public int UserId { get; set; }
    public AppUser User { get; set; }
    
    public int CourseId { get; set; }
    public Course Course { get; set; }

    public decimal SubscriptionPrice { get; set; }
    public int DurationInMonths { get; set; }
    public DateTime ExpirationDate { get; set; }
} 