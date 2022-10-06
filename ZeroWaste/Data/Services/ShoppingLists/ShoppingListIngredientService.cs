using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.ShoppingLists;

public class ShoppingListIngredientService : IShoppingListIngredientService
{
    private readonly AppDbContext _context;
    public ShoppingListIngredientService(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddIngredientToShoppingList(int shoppingListId, int ingredientId)
    {
        ShoppingListIngredient entity = new ShoppingListIngredient
        {
            IngredientId = ingredientId,
            ShoppingListId = shoppingListId,
            Quantity = 0
        };
        await _context.ShoppingListIngredients.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public Task AddIngredientToShoppingList(int shoppingListId, int ingredientId, int quantity)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteIngredientFromShoppingList(int shoppingId, int ingredientId)
    {
        var entity = await _context
            .ShoppingListIngredients
            .Where(x => x.ShoppingListId == shoppingId && x.Id == ingredientId)
            .FirstOrDefaultAsync();
        EntityEntry entityEntry = _context.Entry(entity);
        entityEntry.State = EntityState.Deleted;
        await _context.SaveChangesAsync();
    }

    public async Task<List<ShoppingListIngredient>> GetIngredientsForShoppingList(int shoppingListId)
    {
        var list = await _context
            .ShoppingListIngredients
            .Include(x => x.Ingredient)
            .ThenInclude(x => x.UnitOfMeasure)
            .Where(x => x.ShoppingListId == shoppingListId && x.Quantity == 0)
            .ToListAsync();
        return list;
    }
}