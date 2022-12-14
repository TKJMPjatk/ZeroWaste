using AutoMapper;
using ZeroWaste.Data.Enums;
using ZeroWaste.Data.Services.RecipesSearch;
using ZeroWaste.Data.Static;
using ZeroWaste.Data.Structs;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers.SearchRecipeStrategy;

public class SearchForConfirm : ISearchRecipeStrategy
{
    private readonly IRecipesSearchService _recipesSearchService;
    public SearchForConfirm(IRecipesSearchService recipesSearchService)
    {
        _recipesSearchService = recipesSearchService;
    }
    public async Task<List<RecipeResult>> SearchRecipe(SearchRecipeResultsVm searchRecipeResultsVm)
    {
        var recipeResult = await _recipesSearchService
            .GetByStatus1(searchRecipeResultsVm.StatusId);
        return recipeResult.MapToRecipeResult();
    }
    public SearchType GetSearchType(SearchRecipeResultsVm searchRecipeResultsVm)
    {
        return SearchType.Admin;
    }
}