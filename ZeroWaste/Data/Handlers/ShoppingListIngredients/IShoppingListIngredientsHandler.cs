using ZeroWaste.Data.ViewModels.ShoppingList;

namespace ZeroWaste.Data.Handlers.ShoppingListIngredients;

public interface IShoppingListIngredientsHandler
{
    Task<ShoppingListIngredientsVm> GetShoppingListIngredientsVm(int shoppingListId, string searchString);
    Task EditQuantityOfNewIngredients(EditQuantityVM editQuantityVm);
    Task<int> ChangeShoppingListIngredientSelection(int shoppingListIngredientId);
}