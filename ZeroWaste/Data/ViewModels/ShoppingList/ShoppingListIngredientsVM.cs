using ZeroWaste.Data.ViewModels.Ingredients;

namespace ZeroWaste.Data.ViewModels.ShoppingList;

public class ShoppingListIngredientsVM
{
    public int ShoppingListId { get; set; }
    public List<NewIngredientShoppingListVM> IngredientShoppingListVms { get; set; }
}