using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.ShoppingLists;

public interface IIngredientSelectionService
{
    Task SelectIngredient(ShoppingListIngredient ingredient);
    Task UnSelectIngredient(ShoppingListIngredient ingredient);
    Task<ShoppingListIngredient> GetShoppingListIngredientByIdAsync(int id);
}