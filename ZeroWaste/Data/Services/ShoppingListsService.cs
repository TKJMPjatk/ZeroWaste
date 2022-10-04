using Microsoft.EntityFrameworkCore;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services;

public class ShoppingListsService : IShoppingListsService
{
    private AppDbContext _context;
    public ShoppingListsService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<ShoppingList>> GetAllAsync()
    {
        return await _context
            .ShoppingLists
            .Include(x => x.ShoppingListIngredients)
            .ToListAsync();
    }

    public async Task<ShoppingList> GetByIdAsync(int id)
    {
        return await _context
            .ShoppingLists
            .Include(x => x.ShoppingListIngredients)
            .ThenInclude(x => x.Ingredient)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }
}