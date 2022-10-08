using ZeroWaste.Data.ViewModels.ShoppingList;

namespace ZeroWaste.Data.Helpers;

public interface IShoppingListIngredientsHelper
{
    Task<ShoppingListIngredientsVm> GetShoppingListIngredients(int id);
}