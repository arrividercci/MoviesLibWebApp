using System.ComponentModel.DataAnnotations;

namespace MoviesWebAspNetMVC.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name= "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Введіть логін")]
        [Display(Name= "Логін")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        [Display(Name = "Підтвердження паролю")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
