using ZeroWaste.DapperModels;
using ZeroWaste.Data.Structs;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.Recipes;
using ZeroWaste.Data.ViewModels.RecipeSearch;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.RecipesSearch;

public interface IRecipesSearchService
{
    Task<List<SearchRecipesResults>> GetByCategoryAsync(int categoryId, string userId);
    Task<List<SearchByIngredientsResults>> GetByIngredients(List<IngredientForSearch> searchByIngredientsVm, string userId);
    Task<List<SearchByIngredientsResults>> GetByAll(List<IngredientForSearch> searchByIngredientsVm, int categoryId, string userId);
    Task<List<Recipe>> GetByStatus(int statusId);
    Task<List<Recipe>> GetFavouriteByUserIdAsync(string userId);
    Task<List<Recipe>> GetHatedByUserIdAsync(string userId);
    Task<List<Recipe>> GetEditMineByUserAsync(string userId);
    Task<List<SearchRecipesResults>> GetByStatus1(int statusId);
    Task<List<SearchRecipesResults>> GetFavouriteByUserIdAsync1(string userId);
    Task<List<SearchRecipesResults>> GetHatedByUserIdAsync1(string userId);
    Task<List<SearchRecipesResults>> GetEditMineByUserAsync1(string userId);

}