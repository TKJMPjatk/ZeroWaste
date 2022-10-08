using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers;

public interface IShoppingListHandler
{
    Task<ShoppingList> Create(NewShoppingListVM shoppingListVm);
    Task HandleSelection(int ingredientId);
}