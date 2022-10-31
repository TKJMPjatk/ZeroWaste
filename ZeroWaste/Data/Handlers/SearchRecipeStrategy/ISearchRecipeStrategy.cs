using ZeroWaste.Data.Enums;
using ZeroWaste.Data.Structs;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.RecipeSearch;

namespace ZeroWaste.Data.Handlers.SearchRecipeStrategy;

public interface ISearchRecipeStrategy
{
    Task<List<RecipeResult>> SearchRecipe(SearchRecipeResultsVm searchRecipeResultsVm);
    SearchType GetSearchType(SearchRecipeResultsVm searchRecipeResultsVm);
    
}