using Elomoas.Domain.Common;

namespace Elomoas.Domain.Entities
{
    public class Message : BaseAuditableEntity
    {
        public string Content { get; set; }
        public string SenderId { get; set; }
        public bool IsRead { get; set; }
        public int ChatId { get; set; }
        public virtual Chat Chat { get; set; }
    }
} 