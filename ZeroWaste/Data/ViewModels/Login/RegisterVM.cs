using System.ComponentModel.DataAnnotations;

namespace ZeroWaste.Data.ViewModels.Login;

public class RegisterVm
{
    [Display(Name = "Imię i nazwisko")]
    [Required(ErrorMessage="Pole imię i nazwisko jest wymagane")]
    public string FullName { get; set; }
    [Display(Name = "Adres email")]
    [Required(ErrorMessage="Pole email jest wymagane")]
    [EmailAddress(ErrorMessage = "Adres email ma niepoprawny format")]
    public string EmailAddress { get; set; }
    [Display(Name = "Hasło")]
    [Required(ErrorMessage="Pole hasło jest wymagane")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Display(Name = "Potwierdź hasło")]
    [Required(ErrorMessage="Pole potwierdź hasło jest wymagane")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Hasła nie pasują")]
    public string ConfirmPassword { get; set; }
}