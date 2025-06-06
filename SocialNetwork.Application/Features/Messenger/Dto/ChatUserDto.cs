namespace Elomoas.Application.Features.Messenger.Dto;

public class ChatUserDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Img { get; set; }
    public string LastMessage { get; set; }
    public int UnreadCount { get; set; }
    public bool IsTyping { get; set; }
} 