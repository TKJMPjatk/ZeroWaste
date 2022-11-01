using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.CategorySearch;
using ZeroWaste.Data.ViewModels.RecipeSearch;

namespace ZeroWaste.Data.Handlers.SearchRecipesHandlers;

public interface ISearchRecipeHandler
{
    Task<List<CategorySearchVm>> GetCategoriesSearchVm();
    SearchByIngredientsVm AddIngredient(SearchByIngredientsVm searchByIngredientsVm);
    Task<SearchRecipeResultsVm> GetSearchRecipeResultVmByIngredients(SearchByIngredientsVm searchByIngredientsVm);
    Task<SearchRecipeResultsVm> GetSearchRecipeResultVmByCategory(int categoryId);
    Task<SearchRecipeResultsVm> GetSearchRecipeResultVmForConfirm(int statusId);
    Task<SearchRecipeResultsVm> GetSearchRecipeResultVmFavourite(string userId);
    Task<SearchRecipeResultsVm> GetSearchRecipeResultVmFiltered(SearchRecipeResultsVm searchRecipeResultsVm);
    Task<SearchRecipeResultsVm> GetSearchRecipeResultVmSorted(SearchRecipeResultsVm resultsVm);
}