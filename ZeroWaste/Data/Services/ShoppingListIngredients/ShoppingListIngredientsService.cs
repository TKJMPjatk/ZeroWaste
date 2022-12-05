using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ZeroWaste.Data.DapperConnection;
using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.ShoppingListIngredients;

public class ShoppingListIngredientsService : IShoppingListIngredientsService
{
    private readonly AppDbContext _context;
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public ShoppingListIngredientsService(AppDbContext context, IDbConnectionFactory dbConnectionFactory)
    {
        _context = context;
        _dbConnectionFactory = dbConnectionFactory;
    }
    public async Task AddAsync(int shoppingListId, int ingredientId)
    {
        ShoppingListIngredient entity = new ShoppingListIngredient
        {
            IngredientId = ingredientId,
            ShoppingListId = shoppingListId,
            Quantity = 0
        };
        await _context.ShoppingListIngredients.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int shoppingListId, int ingredientId)
    {
        var entity = await _context.ShoppingListIngredients.FirstOrDefaultAsync(x =>
            x.IngredientId == ingredientId && x.ShoppingListId == shoppingListId);
        EntityEntry entityEntry = _context.Entry(entity);
        entityEntry.State = EntityState.Deleted;
        await _context.SaveChangesAsync();
    }

    public async Task EditQuantityAsync(int ingredientId, double quantity)
    {
        var entity = await _context
            .ShoppingListIngredients
            .FirstOrDefaultAsync(x => x.Id == ingredientId);
        entity.Quantity = quantity;
        EntityEntry entityEntry = _context.Entry(entity);
        entityEntry.State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    public async Task<int> DeleteByIdAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        EntityEntry entityEntry = _context.Entry(entity);
        entityEntry.State = EntityState.Deleted;
        await _context.SaveChangesAsync();
        return entity.ShoppingListId;
    }
    private async Task<ShoppingListIngredient> GetByIdAsync(int id)
    {
        return await _context.ShoppingListIngredients
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task<List<ShoppingListIngredient>> GetByShoppingListIdAsync(int shoppingListId)
    {
        var list = await _context
            .ShoppingListIngredients
            .Include(x => x.Ingredient)
            .ThenInclude(x => x.UnitOfMeasure)
            .Where(x => x.ShoppingListId == shoppingListId)
            .ToListAsync();
        return list;
    }
    public async Task<int> ChangeSelection(int shoppingListIngredientId)
    {
        int shoppingListId;
        using(var connection = _dbConnectionFactory.GetDbConnection())
        {
            string procedureName = "dbo.ChangeShoppingListIngredientSelection";
            var parameters = new DynamicParameters();
            parameters.Add("ShoppingListIngredientId", shoppingListIngredientId, DbType.Int32, ParameterDirection.Input);
            shoppingListId = await connection.QueryFirstOrDefaultAsync<int>(procedureName, parameters,
                commandType: CommandType.StoredProcedure);
        }
        return shoppingListId;
    }
}