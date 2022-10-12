using Microsoft.EntityFrameworkCore;
using ZeroWaste.Data.Helpers;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.RecipeIngredients;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.Recipes
{
    public class RecipesService : IRecipesService
    {
        private readonly AppDbContext _context;
        private readonly IRecipeMapperHelper _mapperHelper;

        public RecipesService(AppDbContext context, IRecipeMapperHelper mapperHelper)
        {
            _context = context;
            _mapperHelper = mapperHelper;
        }

        public async Task AddNewAsync(NewRecipeVM newRecipeVM)
        {
            Recipe recipe = _mapperHelper.Map(newRecipeVM);
            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
        }
    }
}
