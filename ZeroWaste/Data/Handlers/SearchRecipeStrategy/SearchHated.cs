using AutoMapper;
using ZeroWaste.Data.Enums;
using ZeroWaste.Data.Services.RecipesSearch;
using ZeroWaste.Data.Static;
using ZeroWaste.Data.Structs;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers.SearchRecipeStrategy;

public class SearchHated : ISearchRecipeStrategy
{    
    private readonly IRecipesSearchService _recipesSearchService;
    public SearchHated(IRecipesSearchService recipesSearchService)
    {
        _recipesSearchService = recipesSearchService;
    }
    public async Task<List<RecipeResult>> SearchRecipe(SearchRecipeResultsVm searchRecipeResultsVm)
    {
        var list = await _recipesSearchService.GetHatedByUserIdAsync(searchRecipeResultsVm.UserId);
        return list.MapToRecipeResult();
    }
    public SearchType GetSearchType(SearchRecipeResultsVm searchRecipeResultsVm)
    {
        return SearchType.Hated;
    }
}