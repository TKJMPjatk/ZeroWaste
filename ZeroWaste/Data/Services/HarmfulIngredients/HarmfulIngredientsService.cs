using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.HarmfulIngredients
{
    public class HarmfulIngredientsService : IHarmfulIngredientsService
    {
        private readonly AppDbContext _context;
        public HarmfulIngredientsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HarmfulIngredientVM>> GetHarmfulIngredientsForUser(string userId)
        {
            var parameter = new { UserId = userId };
            var query = @"SELECT	IngredientId,
		                            IngredientName,
		                            IngredientType
                            FROM    [dbo].[IngredientsHarmfulForUser] (@UserId)
                            ORDER BY    IngredientName ASC";
            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            var IngredientsHarmfulForUserResult = await connection.QueryAsync<HarmfulIngredientVM>(query, parameter);
            return IngredientsHarmfulForUserResult;
        }

        public async Task<IEnumerable<HarmfulIngredientVM>> GetSafeIngredientsForUser(string userId)
        {
            var parameter = new { UserId = userId };
            var query = @"SELECT	IngredientId,
		                            IngredientName,
		                            IngredientType
                            FROM    [dbo].[IngredientsSafeForUser] (@UserId)
                            ORDER BY    IngredientName ASC";
            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            var IngredientsSafeForUserResult = await connection.QueryAsync<HarmfulIngredientVM>(query, parameter);
            return IngredientsSafeForUserResult;
        }

        public async Task MarkIngredientAsHarmful(string userId, int ingredientId)
        {
            var harmfulIngredient = await _context.HarmfulIngredients
                .FirstOrDefaultAsync(c => c.UserId == userId && c.IngredientId == ingredientId);
            if (harmfulIngredient is null)
            {
                var newHarmfulIngredient = new HarmfulIngredient()
                {
                    UserId = userId,
                    IngredientId = ingredientId
                };
                await _context.HarmfulIngredients.AddAsync(newHarmfulIngredient);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UnmarkIngredientAsHarmful(string userId, int ingredientId)
        {
            var harmfulIngredient = await _context.HarmfulIngredients
               .FirstOrDefaultAsync(c => c.UserId == userId && c.IngredientId == ingredientId);
            if (harmfulIngredient is not null)
            {
                _context.Entry(harmfulIngredient).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
        }
    }
}
