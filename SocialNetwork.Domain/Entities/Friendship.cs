using Elomoas.Domain.Common;
using Elomoas.Domain.Entities.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elomoas.Domain.Entities
{
    public class Friendship : BaseAuditableEntity
    {


        public string UserId { get; set; }         // IdentityUser.Id
        public string FriendId { get; set; }       // IdentityUser.Id другого пользователя

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        public FriendshipStatus Status { get; set; } = FriendshipStatus.Pending;
    }
}
