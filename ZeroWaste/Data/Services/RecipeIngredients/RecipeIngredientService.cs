using Microsoft.EntityFrameworkCore;
using ZeroWaste.Data.ViewModels.RecipeIngredients;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.RecipeIngredients
{
    public class RecipeIngredientService : IRecipeIngredientService
    {

        private readonly AppDbContext _context;
        public RecipeIngredientService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RecipeIngredient>> GetCurrentIngredientsAsync(int? recipeId)
        {
            var ingredients = await _context.RecipeIngredients.Where(c => c.RecipeId == recipeId).Include(c => c.Ingredient).ToListAsync();
            return ingredients;
        }

        public async Task<RecipeIngredientsDropdownsVM> GetDropdownsValuesAsync()
        {
            var response = new RecipeIngredientsDropdownsVM()
            {
                Ingredients = await _context.Ingredients.Include(c => c.UnitOfMeasure).OrderBy(c => c.Name).ToListAsync(),
                UnitOfMeasures = await _context.UnitOfMeasures.OrderBy(c => c.Name).ToListAsync()
            };
            return response;
        }

    }
}
