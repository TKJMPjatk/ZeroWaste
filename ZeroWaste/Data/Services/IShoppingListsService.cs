using ZeroWaste.Models;

namespace ZeroWaste.Data.Services;

public interface IShoppingListsService
{
    Task<List<ShoppingList>> GetAllAsync();
    Task<ShoppingList> GetByIdAsync(int id);
    
}