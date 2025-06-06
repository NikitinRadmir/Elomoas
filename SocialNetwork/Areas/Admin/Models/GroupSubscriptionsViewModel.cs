using Elomoas.Domain.Entities;
using System;

namespace SocialNetwork.Areas.Admin.Models;

public class GroupSubscriptionsViewModel
{
    public IEnumerable<GroupSubscription> Subscriptions { get; set; }
}

public class CreateGroupSubscriptionViewModel
{
    public int UserId { get; set; }
    public int GroupId { get; set; }
}

public class UpdateGroupSubscriptionViewModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int GroupId { get; set; }
} 