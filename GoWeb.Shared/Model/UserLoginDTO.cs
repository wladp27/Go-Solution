using System.ComponentModel.DataAnnotations;

namespace GoWeb.Shared.Models
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Пожалуйста, введите логин")]
        [Display(Name = "Никнейм")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
