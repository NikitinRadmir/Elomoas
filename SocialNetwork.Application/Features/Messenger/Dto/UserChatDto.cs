using System;

namespace Elomoas.Application.Features.Messenger.Dto;

public class UserChatDto
{
    public int ChatId { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public string UserImage { get; set; }
    public string LastMessage { get; set; }
    public DateTime? LastMessageTime { get; set; }
    public int UnreadCount { get; set; }
} 