using Microsoft.EntityFrameworkCore;
using ZeroWaste.Data.Structs;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.Recipes;
using ZeroWaste.Data.ViewModels.RecipeSearch;
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
            .Where(x => x.CategoryId == categoryId && x.StatusId == 1)
            .ToListAsync();
        return list;
    }

    public async Task<List<Recipe>> GetByIngredients(List<IngredientForSearch> ingredientsForSearchList)
    {
        var list = await _context
            .Recipes
            .Include(x => x.RecipesIngredients)
            .ThenInclude(x=>x.Ingredient)
            .Where(x => x.StatusId == 1)
            .ToListAsync();
        return list;
    }

    public async Task<List<Recipe>> GetByAll(List<IngredientForSearch> searchByIngredientsVm, int categoryId)
    {        
        var list = await _context
            .Recipes
            .Include(x => x.RecipesIngredients)
            .ThenInclude(x=>x.Ingredient)
            .Where(x => x.CategoryId == categoryId && x.StatusId == 1)
            .ToListAsync();
        return list;
    }

    public async Task<List<Recipe>> GetByStatus(int statusId)
    {
        var list = await _context
            .Recipes
            .Include(x => x.RecipesIngredients)
            .ThenInclude(x => x.Ingredient)
            .Where(x => x.StatusId == statusId)
            .ToListAsync();
        return list;
    }

    public async Task<List<Recipe>> GetFavouriteByUserIdAsync(string userId)
    {
        var listFavourite = await _context.FavouriteRecipes.Where(x => x.UserId == userId).ToListAsync();
        var list = await _context
            .Recipes
            .Include(x => x.RecipesIngredients)
            .ThenInclude(x => x.Ingredient)
            .Where(x => listFavourite.Select(x => x.RecipeId).Contains(x.Id) && x.StatusId == 1)
            .ToListAsync();
        return list;
    }
}