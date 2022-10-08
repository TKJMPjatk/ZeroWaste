using Microsoft.EntityFrameworkCore;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.ShoppingLists;

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

    public async Task<ShoppingList> GetAllIngredientsAsync(int id)
    {
        return await _context
            .ShoppingLists
            .Include(x => x.ShoppingListIngredients)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task CreateAsync(ShoppingList shoppingList)
    {
        await _context.ShoppingLists.AddAsync(shoppingList);
        await _context.SaveChangesAsync();
    }
}