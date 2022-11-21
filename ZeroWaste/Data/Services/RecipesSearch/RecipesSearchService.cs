using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ZeroWaste.DapperModels;
using ZeroWaste.Data.Structs;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.Recipes;
using ZeroWaste.Data.ViewModels.RecipeSearch;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.RecipesSearch;

public class RecipesSearchService : IRecipesSearchService
{
    private readonly AppDbContext _context;
    public RecipesSearchService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<Recipe>> GetByCategoryAsync(int categoryId)
    {
        var list = await _context
            .Recipes
            .Include(x => x.RecipesIngredients)
            .ThenInclude(x=>x.Ingredient)
            .Where(x => x.CategoryId == categoryId && x.StatusId == 1)
            .ToListAsync();
        return list;
    }
    public async Task<List<SearchByIngredientsResults>> GetByIngredients(List<IngredientForSearch> ingredientsForSearchList)
    {
        List<string> listOfIds = await GetListOfIds(ingredientsForSearchList);
        string querySql = @"SELECT
	                          RecipeId
	                        , Title 
                            , EstimatedTime
                            , DifficultyLevel 
                            , CategoryId 
                            , IngredientName 
                            , UnitOfMeasureShortcut 
                            , IngredientQuantity 
                            , Match 
                            , MissingIngredientsCount 
                        FROM [dbo].[SearchByIngredients] (@IdsIngredients)";
        using var connection = new SqlConnection(_context.Database.GetConnectionString());
        var searchByIngredientsResult =
            await connection.QueryAsync<SearchByIngredientsResults>(querySql, new { @IdsIngredients = string.Join(",", listOfIds) });
        return searchByIngredientsResult.ToList();
    }

    private async Task<List<string>> GetListOfIds(List<IngredientForSearch> ingredientsForSearch)
    {
        List<string> listOfId = new();
        foreach (var ingredient in ingredientsForSearch)
        {
            listOfId.Add(await _context
                .Ingredients
                .Where(x => x.Name.ToLower() == ingredient.Name.ToLower())
                .Select(x => x.Id.ToString())
                .FirstOrDefaultAsync());
        }
        return listOfId;
    }
    public async Task<List<SearchByIngredientsResults>> GetByAll(List<IngredientForSearch> ingredientsForSearchList, int categoryId)
    {        
        List<string> listOfIds = await GetListOfIds(ingredientsForSearchList);
        string querySql = @"SELECT
	                          RecipeId
	                        , Title 
                            , EstimatedTime
                            , DifficultyLevel 
                            , CategoryId 
                            , IngredientName 
                            , UnitOfMeasureShortcut 
                            , IngredientQuantity 
                            , Match 
                            , MissingIngredientsCount 
                        FROM [dbo].[SearchByIngredientsAndCategory] (@IdsIngredients, @IdCategory)";
        using var connection = new SqlConnection(_context.Database.GetConnectionString());
        var searchByIngredientsResult =
            await connection.QueryAsync<SearchByIngredientsResults>(querySql, new { @IdsIngredients = string.Join(",", listOfIds), @IdCategory = categoryId });
        return searchByIngredientsResult.ToList();
    }
    public async Task<List<Recipe>> GetByStatus(int statusId)
    {
        var list = await _context
            .Recipes
            .Include(x => x.RecipesIngredients)
            .ThenInclude(x => x.Ingredient)
            .Where(x => x.StatusId == statusId)
            .ToListAsync();
        return list;
    }
    public async Task<List<Recipe>> GetFavouriteByUserIdAsync(string userId)
    {
        var listFavourite = await _context.FavouriteRecipes.Where(x => x.UserId == userId).ToListAsync();
        var list = await _context
            .Recipes
            .Include(x => x.RecipesIngredients)
            .ThenInclude(x => x.Ingredient)
            .Where(x => listFavourite.Select(x => x.RecipeId).Contains(x.Id) && x.StatusId == 1)
            .ToListAsync();
        return list;
    }
    public async Task<List<Recipe>> GetHatedByUserIdAsync(string userId)
    {
        var listHated = await _context.HatedRecipes.Where(x => x.UserId == userId).ToListAsync();
        var list = await _context
            .Recipes
            .Include(x => x.RecipesIngredients)
            .ThenInclude(x => x.Ingredient)
            .Where(x => listHated.Select(x => x.RecipeId).Contains(x.Id) && x.StatusId == 1)
            .ToListAsync();
        return list;
    }
    public async Task<List<Recipe>> GetEditMineByUserAsync(string userId)
    {
        var list = await _context
            .Recipes
            .Include(x => x.RecipesIngredients)
            .ThenInclude(x => x.Ingredient)
            .Where(x => x.AuthorId == userId)
            .ToListAsync();
        return list;
    }
}