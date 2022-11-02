

using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ZeroWaste.Data.Enums;
using ZeroWaste.Data.Services.RecipesSearch;
using ZeroWaste.Data.Static;
using ZeroWaste.Data.Structs;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.RecipeSearch;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers.SearchRecipeStrategy;

public class SearchByIngredientsStrategy : ISearchRecipeStrategy
{
    private readonly IRecipesSearchService _recipesSearchService;
    public SearchByIngredientsStrategy(IRecipesSearchService recipesSearchService)
    {
        _recipesSearchService = recipesSearchService;
    }
    public async Task<List<RecipeResult>> SearchRecipe(SearchRecipeResultsVm searchRecipeResultsVm)
    {
        var recipeResult = await _recipesSearchService
            .GetByIngredients(searchRecipeResultsVm.IngredientsLists);
        return recipeResult.MapToRecipeResult();
    }
    public SearchType GetSearchType(SearchRecipeResultsVm searchRecipeResultsVm)
    {
        return SearchType.Ingredients;
    }
}