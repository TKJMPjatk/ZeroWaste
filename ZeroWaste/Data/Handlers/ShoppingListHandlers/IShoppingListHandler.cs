using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers.ShoppingListHandlers;

public interface IShoppingListHandler
{
    Task<ShoppingList> Create(NewShoppingListVM shoppingListVm);
    Task HandleSelection(int ingredientId);
    Task DeleteAsync(int id);
    Task<bool> IsZeroQuantityIngredientsExists(int shoppingListId);
}