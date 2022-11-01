using System.ComponentModel.DataAnnotations;

namespace ZeroWaste.Data.ViewModels.Login
{
    public class ChangePasswordVM
    {
        [Display(Name = "Stare hasło")]
        [Required(ErrorMessage = "Stare hasło jest wymagane")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Display(Name = "Nowe hasło")]
        [Required(ErrorMessage = "Nowe hasło jest wymagane")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Display(Name = "Potwierdź hasło")]
        [Required(ErrorMessage = "Potwierdzenie nowego hasła jest wymagane")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Hasła nie pasują")]
        public string NewPasswordConfirmed { get; set; }
    }
}
