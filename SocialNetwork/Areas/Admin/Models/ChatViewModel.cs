using System;
using System.Collections.Generic;

namespace SocialNetwork.Areas.Admin.Models;

public class ChatDetailsViewModel
{
    public int Id { get; set; }
    public string User1Id { get; set; }
    public string User1Email { get; set; }
    public string User1Name { get; set; }
    public string User2Id { get; set; }
    public string User2Email { get; set; }
    public string User2Name { get; set; }
    public IEnumerable<ChatMessageViewModel> Messages { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}

public class ChatMessageViewModel
{
    public int Id { get; set; }
    public string SenderId { get; set; }
    public string SenderEmail { get; set; }
    public string SenderName { get; set; }
    public string Content { get; set; }
    public bool IsRead { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
} 