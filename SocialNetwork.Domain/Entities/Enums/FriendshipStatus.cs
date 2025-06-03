using System.ComponentModel.DataAnnotations;

namespace Elomoas.Domain.Entities.Enums;

public enum FriendshipStatus
{
    [Display(Name = "Pending")]
    Pending,
    [Display(Name = "Accepted")]
    Accepted,
    [Display(Name = "Rejected")]
    Rejected,
    [Display(Name = "Blocked")]
    Blocked
} 