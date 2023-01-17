using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ZeroWaste.DapperModels;
using ZeroWaste.Data.DapperConnection;
using ZeroWaste.Data.Structs;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.Recipes;
using ZeroWaste.Data.ViewModels.RecipeSearch;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.RecipesSearch;

public class RecipesSearchService : IRecipesSearchService
{
    private readonly AppDbContext _context;
    private readonly IDbConnectionFactory _connectionFactory;
    public RecipesSearchService(AppDbContext context, IDbConnectionFactory connectionFactory)
    {
        _context = context;
        _connectionFactory = connectionFactory;
    }
    public async Task<List<SearchRecipesResults>> GetByCategoryAsync(int categoryId, string userId)
    {
        string querySql = @"SELECT 
                                  RecipeId 
	                            , Title
	                            , EstimatedTime	
                                , DifficultyLevel
	                            , IngredientName
                                , Stars
                            FROM [dbo].[SearchByCategory] (@CategoryId, @UserId)";
        using var connection = _connectionFactory.GetDbConnection();
        var searchByCategory = await connection.QueryAsync<SearchRecipesResults>
        (
            querySql,
            new { @CategoryId = categoryId, @UserId = userId}
        );
        return searchByCategory.ToList();
    }
    public async Task<List<SearchByIngredientsResults>> GetByIngredients(List<IngredientForSearch> ingredientsForSearchList, string userId)
    {
        List<string> listOfIds = await GetListOfIds(ingredientsForSearchList);
        string querySql = @"SELECT
	                          RecipeId
	                        , Title 
                            , EstimatedTime
                            , DifficultyLevel 
                            , Stars
                            , CategoryId 
                            , IngredientName 
                            , UnitOfMeasureShortcut 
                            , IngredientQuantity 
                            , Match 
                            , MissingIngredientsCount 
                        FROM [dbo].[SearchByIngredients] (@IdsIngredients, @UserId)";
        using var connection = new SqlConnection(_context.Database.GetConnectionString());
        var searchByIngredientsResult =
            await connection.QueryAsync<SearchByIngredientsResults>(querySql, new { @IdsIngredients = string.Join(",", listOfIds), @UserId = userId});
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
    public async Task<List<SearchByIngredientsResults>> GetByAll(List<IngredientForSearch> ingredientsForSearchList, int categoryId, string userId)
    {        
        List<string> listOfIds = await GetListOfIds(ingredientsForSearchList);
        string querySql = @"SELECT
	                          RecipeId
	                        , Title 
                            , EstimatedTime
                            , DifficultyLevel
                            , Stars
                            , CategoryId 
                            , IngredientName 
                            , UnitOfMeasureShortcut 
                            , IngredientQuantity 
                            , Match 
                            , MissingIngredientsCount 
                        FROM [dbo].[SearchByIngredientsAndCategory_2] (@IdsIngredients, @IdCategory, @UserId)";
        using var connection = new SqlConnection(_context.Database.GetConnectionString());
        var searchByIngredientsResult =
            await connection.QueryAsync<SearchByIngredientsResults>(querySql, new { @IdsIngredients = string.Join(",", listOfIds), @IdCategory = categoryId, @UserId = userId });
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

    public async Task<List<SearchRecipesResults>> GetByStatus1(int statusId)
    {
        string querySql = @"SELECT 
                                  RecipeId 
	                            , Title
	                            , EstimatedTime	
                                , DifficultyLevel
	                            , IngredientName
                                , Stars
                            FROM [dbo].[SearchRecipesByStatus] (@StatusId)";
        using var connection = _connectionFactory.GetDbConnection();
        var searchByCategory = await connection.QueryAsync<SearchRecipesResults>
        (
            querySql,
            new { @StatusId = statusId}
        );
        return searchByCategory.ToList();
    }

    public async Task<List<SearchRecipesResults>> GetFavouriteByUserIdAsync1(string userId)
    {
        string querySql = @"SELECT 
                                  RecipeId 
	                            , Title
	                            , EstimatedTime	
                                , DifficultyLevel
	                            , IngredientName
                                , Stars
                            FROM [dbo].[SearchFavouriteRecipesByUserId] (@UserId)";
        using var connection = _connectionFactory.GetDbConnection();
        var searchByCategory = await connection.QueryAsync<SearchRecipesResults>
        (
            querySql,
            new { @UserId = userId}
        );
        return searchByCategory.ToList();
    }

    public async Task<List<SearchRecipesResults>> GetHatedByUserIdAsync1(string userId)
    {
        string querySql = @"SELECT 
                                  RecipeId 
	                            , Title
	                            , EstimatedTime	
                                , DifficultyLevel
	                            , IngredientName
                                , Stars
                            FROM [dbo].[SearchHatedRecipesByUserId] (@UserId)";
        using var connection = _connectionFactory.GetDbConnection();
        var searchByCategory = await connection.QueryAsync<SearchRecipesResults>
        (
            querySql,
            new { @UserId = userId}
        );
        return searchByCategory.ToList();
    }

    public async Task<List<SearchRecipesResults>> GetEditMineByUserAsync1(string userId)
    {
        string querySql = @"SELECT 
                                  RecipeId 
	                            , Title
	                            , EstimatedTime	
                                , DifficultyLevel
	                            , IngredientName
                                , Stars
                            FROM [dbo].[SearchMineRecipesByUserId] (@UserId)";
        using var connection = _connectionFactory.GetDbConnection();
        var searchByCategory = await connection.QueryAsync<SearchRecipesResults>
        (
            querySql,
            new { @UserId = userId}
        );
        return searchByCategory.ToList();
    }
}