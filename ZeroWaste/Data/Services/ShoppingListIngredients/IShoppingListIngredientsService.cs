using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.ShoppingListIngredients;

public interface IShoppingListIngredientsService
{
    Task AddAsync(int shoppingListId, int ingredientId);
    Task DeleteAsync(int shoppingListId, int ingredientId);
    Task EditQuantityAsync(int ingredientId, double quantity);
    Task<int> DeleteByIdAsync(int id);
    Task<List<ShoppingListIngredient>> GetByShoppingListIdAsync(int shoppingListId);
    Task<int> ChangeSelection(int shoppingListIngredientId);
}