using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Areas.Admin.Models
{
    public class UpdateAppUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Image URL")]
        public string? Img { get; set; }

        [Display(Name = "New Password")]
        public string? NewPassword { get; set; }

    }
} 