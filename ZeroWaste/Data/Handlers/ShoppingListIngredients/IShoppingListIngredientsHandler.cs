using ZeroWaste.Data.ViewModels.ShoppingList;

namespace ZeroWaste.Data.Handlers.ShoppingListIngredients;

public interface IShoppingListIngredientsHandler
{
    Task<ShoppingListIngredientsVm> GetShoppingListIngredientsVm(int shoppingListId, string searchString, int? categoryId);
    Task EditQuantityOfNewIngredients(EditQuantityVM editQuantityVm);
    Task<int> ChangeShoppingListIngredientSelection(int shoppingListIngredientId);
    Task<int> HandleDeleteIngredientFromShoppingList(int shoppingListIngredientId);
    Task AddIngredientToShoppingList(int ingredientId, int shoppingListId);
    Task<EditQuantityVM> GetEditQuantity(int shoppingListId);
    Task DeleteIngredientFromShoppingList(int ingredientId, int shoppingListId);
}