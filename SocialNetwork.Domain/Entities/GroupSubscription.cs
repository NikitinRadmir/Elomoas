using Elomoas.Domain.Common;

namespace Elomoas.Domain.Entities;

public class GroupSubscription : BaseAuditableEntity
{
    public int UserId { get; set; }
    public AppUser User { get; set; }
    
    public int GroupId { get; set; }
    public Group Group { get; set; }
} 