using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers.AutomatedShoppingList;

public interface IAutomatedShoppingListHandler
{
    Task<ShoppingList> AddNewShoppingList(int recipeId);
}