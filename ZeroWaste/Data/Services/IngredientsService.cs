using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ZeroWaste.Data.Helpers;
using ZeroWaste.Data.ViewModels.NewIngredient;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services
{
    public class IngredientsService : IIngredientsService
    {
        private readonly AppDbContext _context;
        private readonly IIngredientMapperHelper _mapperHelper;

        public IngredientsService(AppDbContext context, IIngredientMapperHelper mapperHelper)
        {
            _context = context;
            _mapperHelper = mapperHelper;
        }

        public async Task AddNewAsync(NewIngredientVM newIngredient)
        {
            Ingredient ingredient = _mapperHelper.Map(newIngredient);
            await _context.Ingredients.AddAsync(ingredient);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int? id)
        {
            var ingredient = await _context.Set<Ingredient>().FirstOrDefaultAsync(n => n.Id == id);
            EntityEntry entityEntry = _context.Entry<Ingredient>(ingredient);
            entityEntry.State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Ingredient>> GetAllAsync()
        {
            return await _context.Ingredients
                .Include(i => i.IngredientType)
                .Include(i => i.UnitOfMeasure)
                .ToListAsync();
        }

        public async Task<List<Ingredient>> GetAllAsync(string searchString)
        {
            return await _context.Ingredients
                .Where(i => i.Name.Contains(searchString))
                .Include(i => i.IngredientType)
                .Include(i => i.UnitOfMeasure)
                .ToListAsync();
        }

        public async Task<Ingredient> GetByIdAsync(int? id)
        {
            var ingredientDetails = await _context.Ingredients
                .Where(n => n.Id == id)
                .Include(i => i.IngredientType)
                .Include(i => i.UnitOfMeasure)
                .FirstOrDefaultAsync();

            return ingredientDetails;
        }

        public async Task<NewIngredientDropdownsWM> GetNewIngredientDropdownsWM()
        {
            var response = new NewIngredientDropdownsWM()
            {
                IngredientTypes = await _context.IngredientTypes.OrderBy(n => n.Name).ToListAsync(),
                UnitOfMeasures = await _context.UnitOfMeasures.OrderBy(n => n.Name).ToListAsync()
            };
            return response;
        }

        public async Task<NewIngredientVM> GetVmByIdAsync(int? id)
        {
            var ingredientDetails = await _context.Ingredients
                .Where(n => n.Id == id)
                .Include(i => i.IngredientType)
                .Include(i => i.UnitOfMeasure)
                .FirstOrDefaultAsync();

            var ingredient = _mapperHelper.Map(ingredientDetails);

            return ingredient;
        }

        public async Task UpdateAsync(NewIngredientVM updatedIngredient)
        {
            var ingredient = _mapperHelper.Map(updatedIngredient);
            EntityEntry entityEntry = _context.Entry(ingredient);
            entityEntry.State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
