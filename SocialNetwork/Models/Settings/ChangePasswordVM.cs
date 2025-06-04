using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Elomoas.mvc.Models.Settings
{
    public class ChangePasswordVM
    {
        [Required(ErrorMessage = "The current password is required")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "The new password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "The password must contain at least 6 characters")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Password confirmation is required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
} 