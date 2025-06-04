using Elomoas.Domain.Common;
using Elomoas.Domain.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Elomoas.Domain.Entities;

public class Friendship : BaseAuditableEntity
{
    [Key]
    public int FriendshipId { get; set; }

    public string UserId { get; set; }         
    public string FriendId { get; set; }       

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    public FriendshipStatus Status { get; set; } = FriendshipStatus.Pending;
} 