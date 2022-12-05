using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ZeroWaste.Data.DapperConnection;
using ZeroWaste.Data.Helpers;
using ZeroWaste.Data.ViewModels.NewIngredient;
using ZeroWaste.Data.ViewModels.ShoppingListIngredients;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services
{
    public class IngredientsService : IIngredientsService
    {
        private readonly AppDbContext _context;
        private readonly IIngredientMapperHelper _mapperHelper;
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public IngredientsService(AppDbContext context, IIngredientMapperHelper mapperHelper, IDbConnectionFactory dbConnectionFactory)
        {
            _context = context;
            _mapperHelper = mapperHelper;
            _dbConnectionFactory = dbConnectionFactory;
        }
        public async Task AddNewAsync(NewIngredientVM newIngredient)
        {
            Ingredient ingredient = _mapperHelper.Map(newIngredient);
            await _context.Ingredients.AddAsync(ingredient);
            await _context.SaveChangesAsync();
        }

        public async Task<int> AddNewReturnsIdAsync(NewIngredientVM newIngredient)
        {
            Ingredient ingredient = _mapperHelper.Map(newIngredient);
            await _context.Ingredients.AddAsync(ingredient);
            await _context.SaveChangesAsync();
            return ingredient.Id;
        }

        public async Task DeleteAsync(int? id)
        {
            var ingredient = await _context.Set<Ingredient>().FirstOrDefaultAsync(n => n.Id == id);
            EntityEntry entityEntry = _context.Entry<Ingredient>(ingredient);
            entityEntry.State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task<List<IngredientsToAddVm>> GetIngredientsToAddVmAsync(int shoppingListId, string searchString)
        {        
            List<IngredientsToAddVm> ingredientsToAddVmList = new List<IngredientsToAddVm>();
            using (var connection = _dbConnectionFactory.GetDbConnection())
            {
                string query = @"SELECT 
                                  [Id]
                                , [Name]
                                , [Description]
                                , [IngredientTypeId]
                                , [UnitOfMeasureId]
                                , [IsAdded]
                            FROM dbo.GetIngredientsToAddToShoppingList(@ShoppingListId, @SearchString)";
                var paramateres = new {@ShoppingListId = shoppingListId, @SearchString = searchString == null ? "" : searchString };
                ingredientsToAddVmList = (await connection.QueryAsync<IngredientsToAddVm>(query, paramateres)).ToList();
            }
            return ingredientsToAddVmList.OrderBy(x => x.IsAdded).ToList();
        }

        public async Task<List<Ingredient>> GetAllAsync()
        {
            return await _context.Ingredients
                .Include(i => i.IngredientType)
                .Include(i => i.UnitOfMeasure)
                .OrderBy(i => i.Name)
                .ToListAsync();
        }

        public async Task<List<Ingredient>> GetAllAsync(string searchString)
        {
            return await _context.Ingredients
                .Where(i => i.Name.Contains(searchString))
                .Include(i => i.IngredientType)
                .Include(i => i.UnitOfMeasure)
                .OrderBy(i => i.Name)
                .ToListAsync();
        }

        public async Task<Ingredient?> GetByIdAsync(int? id)
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

        public async Task<NewIngredientVM> GetVmByNameAsync(string name)
        {
            var ingredientDetails = await _context.Ingredients
                .Where(n => n.Name == name)
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

        public async Task<bool> IngredientExistsAsync(string ingredientName)
        {
            bool ingredientExists = await _context.Ingredients
                .Where(n => n.Name == ingredientName)
                .AnyAsync();
            return ingredientExists;
        }

        public async Task<Ingredient?> GetByNameAsync(string ingredientName)
        {
            var ingredient = await _context.Ingredients
                .Where(n => n.Name == ingredientName)
                .FirstOrDefaultAsync();
            return ingredient;
        }
    }
}
