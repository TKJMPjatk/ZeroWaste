using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.ShoppingLists;
public interface IShoppingListsService
{
    Task<List<ShoppingList>> GetAllAsync();
    Task<ShoppingList> GetByIdAsync(int id);
    Task<ShoppingList> GetAllIngredientsAsync(int id);
    Task<ShoppingList> CreateAsync(ShoppingList shoppingList);
    Task DeleteAsync(int id);
    Task<bool> IsZeroQuantityIngredientsExists(int shoppingListId);
    Task EditAsync(ShoppingList shoppingList);
}