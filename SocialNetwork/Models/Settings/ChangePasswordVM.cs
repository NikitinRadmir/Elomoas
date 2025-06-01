using System.ComponentModel.DataAnnotations;

namespace Elomoas.mvc.Models.Settings
{
    public class ChangePasswordVM
    {
        [Required(ErrorMessage = "Текущий пароль обязателен")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Новый пароль обязателен")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Подтверждение пароля обязательно")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
} 