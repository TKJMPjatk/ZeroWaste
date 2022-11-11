using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ZeroWaste.Data.Services.HatedRecipes;

public class HatedRecipesService : IHatedRecipesService
{
    private readonly AppDbContext _context;
    public HatedRecipesService(AppDbContext context)
    {
        _context = context;
    }
    public async Task DeleteHatedRecipes(int recipeId, string userId)
    {
        var entity = await _context
            .HatedRecipes
            .FirstOrDefaultAsync(x => x.RecipeId == recipeId && x.UserId == userId);
        EntityEntry entityEntry = _context.Entry(entity);
        entityEntry.State = EntityState.Deleted;
        await _context.SaveChangesAsync();
    }
}