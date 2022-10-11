using Microsoft.EntityFrameworkCore;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.RecipesSearch;

public class RecipesSearchService : IRecipesSearchService
{
    private readonly AppDbContext _context;
    public RecipesSearchService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<Recipe>> GetByCategoryAsync(int categoryId)
    {
        var list = await _context
            .Recipes
            .Include(x => x.RecipesIngredients)
            .ThenInclude(x=>x.Ingredient)
            .Where(x => x.CategoryId == categoryId)
            .ToListAsync();
        return list;
    }
}