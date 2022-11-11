using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ZeroWaste.Data.Services.FavouriteRecipes;

public class FavouritesRecipesService : IFavouritesRecipesService
{
    private readonly AppDbContext _context;
    public FavouritesRecipesService(AppDbContext context)
    {
        _context = context;
    }
    public async Task DeleteFromFavourite(int recipeId, string userId)
    {
        var entity = await _context
            .FavouriteRecipes
            .FirstOrDefaultAsync(x => x.RecipeId == recipeId && x.UserId == userId);
        EntityEntry entityEntry = _context.Entry(entity);
        entityEntry.State = EntityState.Deleted;
        await _context.SaveChangesAsync();
    }
}