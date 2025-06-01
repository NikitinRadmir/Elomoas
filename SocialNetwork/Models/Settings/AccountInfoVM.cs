using System.ComponentModel.DataAnnotations;

namespace Elomoas.mvc.Models.Settings
{
    public class AccountInfoVM
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [EmailAddress(ErrorMessage = "Неверный формат email")]
        public string? Email { get; set; }

        public string? Description { get; set; }
        public string? Img { get; set; }
    }
} 