using ZeroWaste.DapperModels;
using ZeroWaste.Data.Structs;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.Recipes;
using ZeroWaste.Data.ViewModels.RecipeSearch;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.RecipesSearch;

public interface IRecipesSearchService
{
    Task<List<Recipe>> GetByCategoryAsync(int categoryId);
    Task<List<SearchByIngredientsResults>> GetByIngredients(List<IngredientForSearch> searchByIngredientsVm);
    Task<List<SearchByIngredientsResults>> GetByAll(List<IngredientForSearch> searchByIngredientsVm, int categoryId);
    Task<List<Recipe>> GetByStatus(int statusId);
    Task<List<Recipe>> GetFavouriteByUserIdAsync(string userId);
    Task<List<Recipe>> GetHatedByUserIdAsync(string userId);
    Task<List<Recipe>> GetEditMineByUserAsync(string userId);
}