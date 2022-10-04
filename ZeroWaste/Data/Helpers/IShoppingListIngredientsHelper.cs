using ZeroWaste.Data.ViewModels.ShoppingList;

namespace ZeroWaste.Data.Helpers;

public interface IShoppingListIngredientsHelper
{
    Task<ShoppingListIngredientsVM> GetShoppingListIngredients(int id);
}