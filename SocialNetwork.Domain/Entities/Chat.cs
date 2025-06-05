using Elomoas.Domain.Common;
using System.Collections.Generic;

namespace Elomoas.Domain.Entities
{
    public class Chat : BaseAuditableEntity
    {
        public string User1Id { get; set; }
        public string User2Id { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

        public Chat()
        {
            Messages = new List<Message>();
        }
    }
} 