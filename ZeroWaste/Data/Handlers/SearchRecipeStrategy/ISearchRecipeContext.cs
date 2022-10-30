using ZeroWaste.Data.ViewModels;

namespace ZeroWaste.Data.Handlers.SearchRecipeStrategy;

public interface ISearchRecipeContext
{
    Task<SearchRecipeResultsVm> GetSearchRecipeResultVm(SearchRecipeResultsVm recipeResultsVm);
}