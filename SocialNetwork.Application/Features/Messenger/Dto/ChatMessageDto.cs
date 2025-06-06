using System;

namespace Elomoas.Application.Features.Messenger.Dto;

public class ChatMessageDto
{
    public int Id { get; set; }
    public int ChatId { get; set; }
    public string Content { get; set; }
    public string SenderId { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedDate { get; set; }
} 