using AutoMapper;
using ZeroWaste.Data.Enums;
using ZeroWaste.Data.Services.RecipesSearch;
using ZeroWaste.Data.Static;
using ZeroWaste.Data.Structs;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers.SearchRecipeStrategy;

public class SearchByAllStrategy : ISearchRecipeStrategy
{    
    private readonly IRecipesSearchService _recipesSearchService;

    public SearchByAllStrategy(IRecipesSearchService recipesSearchService)
    {
        _recipesSearchService = recipesSearchService;
    }
    public async Task<List<RecipeResult>> SearchRecipe(SearchRecipeResultsVm searchRecipeResultsVm)
    {
        var recipeResult = await _recipesSearchService
            .GetByAll(searchRecipeResultsVm.IngredientsLists, searchRecipeResultsVm.CategoryId);
        return recipeResult.MapToRecipeResult();
    }
    public SearchType GetSearchType(SearchRecipeResultsVm searchRecipeResultsVm)
    {
        //if (IsRecipeListNullOrEmpty(searchRecipeResultsVm.IngredientsLists))
            return SearchType.Categories;
        return SearchType.Ingredients;
    }
}