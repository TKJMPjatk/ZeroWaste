using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ZeroWaste.Data.Static;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.ShoppingLists;

public class ShoppingListsService : IShoppingListsService
{
    private AppDbContext _context;
    public ShoppingListsService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<ShoppingList>> GetByUserIdAsync(string userId)
    {        
        return await _context
        .ShoppingLists
        .Include(x => x.ShoppingListIngredients)
        .Where(x => x.UserId == userId)
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

    public async Task<ShoppingList> GetByShoppingListIngredientIdAsync(int shoppingListIngredientId)
    {
        return await _context
            .ShoppingListIngredients
            .Include(x => x.ShoppingList)
            .Where(x => x.Id == shoppingListIngredientId)
            .Select(x => x.ShoppingList)
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
    public async Task<ShoppingList> CreateAsync(ShoppingList shoppingList)
    {
        await _context.ShoppingLists.AddAsync(shoppingList);
        //Todo: Identity
        await _context.SaveChangesAsync();
        return shoppingList;
    }
    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        EntityEntry entityEntry = _context.Entry(entity);
        entityEntry.State = EntityState.Deleted;
        await _context.SaveChangesAsync();
    }
    public async Task<bool> IsZeroQuantityIngredientsExists(int shoppingListId)
    {
        return await _context
            .ShoppingListIngredients
            .AnyAsync(x => x.ShoppingListId == shoppingListId && x.Quantity == 0);
    }
    public async Task EditAsync(ShoppingList shoppingList)
    {
        var entity = await GetByIdAsync(shoppingList.Id);
        entity.Title = shoppingList.Title;
        EntityEntry entityEntry = _context.Entry(entity);
        entityEntry.State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    public async Task<bool> IsShoppingListExists(int shoppingListId)
    {
        return await _context.ShoppingLists.AnyAsync(x => x.Id == shoppingListId);
    }
}