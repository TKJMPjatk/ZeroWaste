using Microsoft.EntityFrameworkCore;
using ZeroWaste.Data.ViewModels.NewIngredient;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services
{
    public class IngredientsService : IIngredientsService
    {
        private readonly AppDbContext _context;

        public IngredientsService(AppDbContext context)
        {
            _context = context;
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

        public async Task<NewIngredientDropdownsWM> GetNewIngredientDropdownsWM()
        {
            var response = new NewIngredientDropdownsWM()
            {
                IngredientTypes = await _context.IngredientTypes.OrderBy(n => n.Name).ToListAsync(),
                UnitOfMeasures = await _context.UnitOfMeasures.OrderBy(n => n.Name).ToListAsync()
            };
            return response;
        }
    }
}
