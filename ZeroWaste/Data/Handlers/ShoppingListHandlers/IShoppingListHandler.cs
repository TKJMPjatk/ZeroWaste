using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers.ShoppingListHandlers;

public interface IShoppingListHandler
{
    Task<List<ShoppingList>> GetShoppingListsByUserId(string userId);
    Task<ShoppingList> GetShoppingListById(int id);
    Task<ShoppingList> Create(NewShoppingListVM shoppingListVm, string userId);
    Task DeleteAsync(int id);
    Task<bool> IsZeroQuantityIngredientsExists(int shoppingListId);
    Task<bool> IsShoppingListExists(int shoppingListId);
}