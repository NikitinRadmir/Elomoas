using Elomoas.Domain.Entities;
using Elomoas.Domain.Entities.Enums;
using System;
using System.Collections.Generic;

namespace SocialNetwork.Areas.Admin.Models;

public class FriendshipsViewModel
{
    public IEnumerable<Friendship> Friendships { get; set; }
}

public class CreateFriendshipViewModel
{
    public string UserId { get; set; }
    public string FriendId { get; set; }
    public FriendshipStatus Status { get; set; }
}

public class UpdateFriendshipViewModel
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string FriendId { get; set; }
    public FriendshipStatus Status { get; set; }
} 