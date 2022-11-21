using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.ShoppingLists;
public interface IShoppingListsService
{
    Task<List<ShoppingList>> GetByUserIdAsync(string userId);
    Task<ShoppingList?> GetByIdAsync(int id);
    Task<ShoppingList?> GetByShoppingListIngredientIdAsync(int shoppingListIngredientId);
    Task<ShoppingList?> GetAllIngredientsAsync(int id);
    Task<ShoppingList> CreateAsync(ShoppingList shoppingList);
    Task DeleteAsync(int id);
    Task<bool> IsZeroQuantityIngredientsExists(int shoppingListId);
    Task EditAsync(ShoppingList shoppingList);
    Task<bool> IsShoppingListExists(int shoppingListId);
}