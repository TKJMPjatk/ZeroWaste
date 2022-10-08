using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.VisualBasic;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.ShoppingLists;

public class IngredientSelectionService : IIngredientSelectionService
{
    private readonly AppDbContext _context;
    public IngredientSelectionService(AppDbContext context)
    {
        _context = context;
    }
    public async Task SelectIngredient(ShoppingListIngredient ingredient)
    {
        ingredient.Selected = true;
        EntityEntry entity = _context.Entry(ingredient);
        entity.State = EntityState.Modified;
        await _context.SaveChangesAsync(); 
    }
    public async Task UnSelectIngredient(ShoppingListIngredient ingredient)
    {
        ingredient.Selected = false;
        EntityEntry entity = _context.Entry(ingredient);
        entity.State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<ShoppingListIngredient> GetShoppingListIngredientByIdAsync(int id)
    {
        var entity = await _context
            .ShoppingListIngredients
            .FirstOrDefaultAsync(x => x.Id == id);
        return entity;
    }
}