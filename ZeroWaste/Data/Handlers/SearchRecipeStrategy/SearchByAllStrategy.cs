using AutoMapper;
using ZeroWaste.Data.Services.RecipesSearch;
using ZeroWaste.Data.Structs;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers.SearchRecipeStrategy;

public class SearchByAllStrategy : ISearchRecipeStrategy
{    
    private readonly IRecipesSearchService _recipesSearchService;
    private readonly IMapper _mapper;

    public SearchByAllStrategy(IRecipesSearchService recipesSearchService, IMapper mapper)
    {
        _recipesSearchService = recipesSearchService;
        _mapper = mapper;
    }
    public async Task<List<RecipeResult>> SearchRecipe(SearchRecipeResultsVm searchRecipeResultsVm)
    {
        var recipeList = await _recipesSearchService
            .GetByAll(searchRecipeResultsVm.IngredientsLists, searchRecipeResultsVm.CategoryId);
        var recipeResultList = GetRecipeResultsList(recipeList);
        return recipeResultList;
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