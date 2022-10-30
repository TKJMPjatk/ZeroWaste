

using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ZeroWaste.Data.Enums;
using ZeroWaste.Data.Services.RecipesSearch;
using ZeroWaste.Data.Structs;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.RecipeSearch;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers.SearchRecipeStrategy;

public class SearchByIngredientsStrategy : ISearchRecipeStrategy
{
    private readonly IRecipesSearchService _recipesSearchService;
    private readonly IMapper _mapper;
    public SearchByIngredientsStrategy(IRecipesSearchService recipesSearchService, IMapper mapper)
    {
        _recipesSearchService = recipesSearchService;
        _mapper = mapper;
    }
    public async Task<List<RecipeResult>> SearchRecipe(SearchRecipeResultsVm searchRecipeResultsVm)
    {
        var recipeList = await _recipesSearchService
            .GetByIngredients(searchRecipeResultsVm.IngredientsLists);
        var recipeResultList = GetRecipeResultsList(recipeList);
        return recipeResultList;
    }

    public SearchType GetSearchType(SearchRecipeResultsVm searchRecipeResultsVm)
    {
        return SearchType.Ingredients;
    }

    private List<RecipeResult> GetRecipeResultsList(List<Recipe> list)
    {
        List<RecipeResult> recipeResultsList = new List<RecipeResult>();        
        foreach (var item in list)
        {
            RecipeResult recipeResult = _mapper.Map<RecipeResult>(item);
            recipeResultsList.Add(recipeResult);
        }
        return recipeResultsList;
    }
}