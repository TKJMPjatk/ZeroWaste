using ZeroWaste.Data.ViewModels.ShoppingList;

namespace ZeroWaste.Data.Handlers;

public interface IShoppingListHandler
{
    Task Create(NewShoppingListVM shoppingListVm);
}