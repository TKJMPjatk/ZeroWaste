using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        public async Task AddIngredientAsync(int recipeId, int ingredientId, double quantity)
        {
            var recipeIngredient = new RecipeIngredient()
            {
                IngredientId = ingredientId,
                Quantity = quantity,
                RecipeId = recipeId
            };
            await _context.RecipeIngredients.AddAsync(recipeIngredient);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var recipeIngredient = await _context.RecipeIngredients.FindAsync(id);
            EntityEntry entityEntry = _context.Entry<RecipeIngredient>(recipeIngredient);
            entityEntry.State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task<RecipeIngredient> GetByIdAsync(int id)
        {
            var recipeIngredient = await _context.RecipeIngredients.Include(c => c.Ingredient).Where(c => c.Id == id).FirstOrDefaultAsync();
            return recipeIngredient;
        }

        public async Task<IEnumerable<RecipeIngredient>> GetCurrentIngredientsAsync(int? recipeId)
        {
            var ingredients = await _context.RecipeIngredients
                .Where(c => c.RecipeId == recipeId)
                .Include(c => c.Ingredient)
                .Include(c => c.Ingredient.UnitOfMeasure)
                .ToListAsync();
            return ingredients;
        }

        public async Task<RecipeIngredientsDropdownsVM> GetDropdownsValuesAsync()
        {
            var ingredients = await _context.Ingredients.Include(c => c.UnitOfMeasure).OrderBy(c => c.Name).ToListAsync();
            var response = new RecipeIngredientsDropdownsVM()
            {
                Ingredients = _mapperHelper.Map(ingredients),
                UnitOfMeasures = await _context.UnitOfMeasures.OrderBy(c => c.Name).ToListAsync(),
                IngredientTypes = await _context.IngredientTypes.OrderBy(c => c.Name).ToListAsync(),
            };
            return response;
        }

        public async Task<bool> RecipeIngredientsExisting(int ingredientId)
        {
            bool ingredientsExists = await _context.RecipeIngredients.Where(c => c.IngredientId == ingredientId).AnyAsync();
            return ingredientsExists;
        }
    }
}
