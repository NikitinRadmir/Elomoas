using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Areas.Admin.Models
{
    public class CreateAppUserViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Image URL")]
        public string? Img { get; set; }
    }
} 