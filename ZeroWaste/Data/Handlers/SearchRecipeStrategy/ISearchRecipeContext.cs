using ZeroWaste.Data.ViewModels;

namespace ZeroWaste.Data.Handlers.SearchRecipeStrategy;

public interface ISearchRecipeContext
{
    Task<SearchRecipeResultsVm> GetSearchByIngredientsVm(SearchRecipeResultsVm recipeResultsVm);
}