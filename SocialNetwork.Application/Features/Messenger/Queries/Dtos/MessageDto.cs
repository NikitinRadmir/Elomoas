namespace Elomoas.Application.Features.Messenger.Queries.Dtos;

public class MessageDto
{
    public int Id { get; set; }
    public int ChatId { get; set; }
    public string SenderId { get; set; }
    public string Content { get; set; }
    public DateTime? CreatedDate { get; set; }
    public bool IsRead { get; set; }
} 