using Elomoas.Domain.Common;
using Elomoas.Domain.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Elomoas.Domain.Entities;

public class Friendship : BaseAuditableEntity
{
    public string UserId { get; set; }
    public string FriendId { get; set; }
    public FriendshipStatus Status { get; set; } = FriendshipStatus.Pending;
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }

    // Navigation properties
    public virtual IdentityUser User { get; set; }
    public virtual IdentityUser Friend { get; set; }
} 