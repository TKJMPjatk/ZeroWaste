using Microsoft.EntityFrameworkCore;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.Recipes;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.RecipesSearch;

public class RecipesSearchService : IRecipesSearchService
{
    private readonly AppDbContext _context;
    public RecipesSearchService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<SearchRecipeResultsVm> GetByCategoryAsync(int categoryId)
    {
        var list = await _context
            .Recipes
            .Include(x => x.RecipesIngredients)
            .ThenInclude(x=>x.Ingredient)
            .Where(x => x.CategoryId == categoryId)
            .ToListAsync();
        SearchRecipeResultsVm resultsVm = new SearchRecipeResultsVm()
        {
            RecipeList = list
        };
        return resultsVm;
    }

    public async Task<SearchRecipeResultsVm> GetByIngredients(List<IngredientsForSearchVM> ingredientsForSearchVm)
    {
        var list = await _context
            .Recipes
            .Include(x => x.RecipesIngredients)
            .ThenInclude(x=>x.Ingredient)
            .ToListAsync();
        SearchRecipeResultsVm resultsVm = new SearchRecipeResultsVm()
        {
            RecipeList = list
        };
        return resultsVm;
    }
}