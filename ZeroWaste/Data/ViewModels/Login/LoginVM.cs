
using System.ComponentModel.DataAnnotations;

namespace ZeroWaste.Data.ViewModels.Login;

public class LoginVm
{
    [Display(Name = "Adres email")]
    [Required(ErrorMessage="Pole email jest wymagane")]
    public string EmailAddress { get; set; }
    [Display(Name = "Hasło")]
    [Required(ErrorMessage="Pole hasło jest wymagane")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}