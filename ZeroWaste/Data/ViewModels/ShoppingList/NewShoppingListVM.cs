using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ZeroWaste.Data.ViewModels.ShoppingList;

public class NewShoppingListVM
{
    [Required]
    [Display(Name = "Tytuł listy zakupów")]
    public string Title { get; set; }
    [Required]
    [Display(Name = "Notatka")]
    public string Note { get; set; }
    public DateTime CreatedAt { get; set; }
}