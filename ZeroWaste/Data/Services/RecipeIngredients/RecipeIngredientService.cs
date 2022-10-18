using Microsoft.EntityFrameworkCore;
using ZeroWaste.Data.Helpers;
using ZeroWaste.Data.ViewModels.RecipeIngredients;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.RecipeIngredients
{
    public class RecipeIngredientService : IRecipeIngredientService
    {

        private readonly AppDbContext _context;
        private readonly IRecipeIngredientMapperHelper _mapperHelper;
        public RecipeIngredientService(AppDbContext context, IRecipeIngredientMapperHelper mapperHelper)
        {
            _context = context;
            _mapperHelper = mapperHelper;
        }

        public async Task<IEnumerable<RecipeIngredient>> GetCurrentIngredientsAsync(int? recipeId)
        {
            var ingredients = await _context.RecipeIngredients.Where(c => c.RecipeId == recipeId).Include(c => c.Ingredient).ToListAsync();
            return ingredients;
        }

        public async Task<RecipeIngredientsDropdownsVM> GetDropdownsValuesAsync()
        {
            var ingredients = await _context.Ingredients.Include(c => c.UnitOfMeasure).OrderBy(c => c.Name).ToListAsync();
            var response = new RecipeIngredientsDropdownsVM()
            {
                Ingredients = _mapperHelper.Map(ingredients),
                UnitOfMeasures = await _context.UnitOfMeasures.OrderBy(c => c.Name).ToListAsync()
            };
            return response;
        }

    }
}
