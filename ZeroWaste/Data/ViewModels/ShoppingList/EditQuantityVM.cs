using ZeroWaste.Models;

namespace ZeroWaste.Data.ViewModels.ShoppingList;

public class EditQuantityVM
{
    public int ShoppingListId { get; set; }
    public List<ShoppingListIngredient>? IngredientsToEditQuantity { get; set; }
}