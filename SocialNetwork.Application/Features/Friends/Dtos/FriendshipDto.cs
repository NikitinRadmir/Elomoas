using System;
using Elomoas.Domain.Entities.Enums;

namespace Elomoas.Application.Features.Friends.Dtos;

public class FriendshipDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string FriendId { get; set; }
    public FriendshipStatus Status { get; set; }
} 