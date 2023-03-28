using System.ComponentModel.DataAnnotations;

namespace MoviesWebAspNetMVC.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Логін")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Запам'ятати?")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
