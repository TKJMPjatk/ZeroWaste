using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.ShoppingLists;

public interface IShoppingListIngredientService
{
    Task AddIngredientToShoppingList(int shoppingListId, int ingredientId);
    Task AddIngredientToShoppingList(int shoppingListId, int ingredientId, int quantity);
    Task DeleteIngredientFromShoppingList(int shoppingId, int ingredientId);
    Task<List<ShoppingListIngredient>> GetIngredientsForShoppingList(int shoppingListId);
}