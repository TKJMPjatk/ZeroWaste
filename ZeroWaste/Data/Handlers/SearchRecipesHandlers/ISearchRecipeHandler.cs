using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.CategorySearch;
using ZeroWaste.Data.ViewModels.RecipeSearch;

namespace ZeroWaste.Data.Handlers.SearchRecipesHandlers;

public interface ISearchRecipeHandler
{
    Task<List<CategorySearchVm>> GetCategoriesSearchVm();
    SearchByIngredientsVm AddIngredient(SearchByIngredientsVm searchByIngredientsVm);
    Task<SearchRecipeResultsVm> GetSearchRecipeResultVmSorted(SearchRecipeResultsVm resultsVm);
    Task<int> GetRandomRecipeId();
}