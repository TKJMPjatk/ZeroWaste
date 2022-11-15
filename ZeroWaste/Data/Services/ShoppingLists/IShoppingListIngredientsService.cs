using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.ShoppingLists;

public interface IShoppingListIngredientsService
{
    Task AddIngredientToShoppingList(int shoppingListId, int ingredientId);
    Task EditQuantityAsync(int ingredientId, double quantity);
    Task DeleteIngredientFromShoppingList(int shoppingId, int ingredientId);
    Task<List<ShoppingListIngredient>> GetIngredientsForShoppingList(int shoppingListId);
    Task<List<ShoppingListIngredient>> GetNewIngredientsForShoppingList(int shoppingListId);
    Task<int> ChangeShoppingListIngredientSelection(int shoppingListIngredientId);
}