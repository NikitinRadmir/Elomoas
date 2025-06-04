using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Elomoas.mvc.Models.Settings
{
    public class AccountInfoVM
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [EmailAddress(ErrorMessage = "The wrong format email")]
        public string? Email { get; set; }

        public string? Description { get; set; }
        public string? Img { get; set; }

        [Display(Name = "Profile Image")]
        public IFormFile? ImageFile { get; set; }
    }
} 