using System.ComponentModel.DataAnnotations;

namespace Elomoas.Domain.Entities
{
    public class Settings
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string Theme { get; set; } = "light";
        public bool NotificationsEnabled { get; set; } = true;
    }
} 