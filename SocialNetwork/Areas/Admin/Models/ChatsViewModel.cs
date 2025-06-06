using Elomoas.Domain.Entities;

namespace SocialNetwork.Areas.Admin.Models;

public class ChatsViewModel
{
    public IEnumerable<Chat> Chats { get; set; }
}

public class CreateChatViewModel
{
    public string User1Id { get; set; }
    public string User2Id { get; set; }
}

public class UpdateChatViewModel
{
    public int Id { get; set; }
    public string User1Id { get; set; }
    public string User2Id { get; set; }
} 