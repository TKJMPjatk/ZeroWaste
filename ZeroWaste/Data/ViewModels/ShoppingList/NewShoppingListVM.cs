using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ZeroWaste.Data.ViewModels.ShoppingList;

public class NewShoppingListVM
{
    [Required(ErrorMessage = "Pole TYTUŁ LISTY ZAKUPÓW jest wymagane")]
    [Display(Name = "Tytuł listy zakupów")]
    public string? Title { get; set; }
    [Display(Name = "Notatka")]
    public string? Note { get; set; }
}