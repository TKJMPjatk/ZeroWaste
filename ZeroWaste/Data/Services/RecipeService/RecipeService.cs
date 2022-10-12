using Microsoft.EntityFrameworkCore;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.RecipeService;

public class RecipeService : IRecipeService
{
    private readonly AppDbContext _context;
    public RecipeService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Recipe> GetByIdAsync(int id)
    {
        var entity = await _context
            .Recipes
            .FirstOrDefaultAsync(x => 
                x.Id == id);
        return entity;
    }
}