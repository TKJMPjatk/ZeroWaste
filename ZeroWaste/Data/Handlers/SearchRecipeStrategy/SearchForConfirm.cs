using AutoMapper;
using ZeroWaste.Data.Enums;
using ZeroWaste.Data.Services.RecipesSearch;
using ZeroWaste.Data.Structs;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers.SearchRecipeStrategy;

public class SearchForConfirm : ISearchRecipeStrategy
{
    private readonly IRecipesSearchService _recipesSearchService;
    private readonly IMapper _mapper;
    public SearchForConfirm(IRecipesSearchService recipesSearchService, IMapper mapper)
    {
        _recipesSearchService = recipesSearchService;
        _mapper = mapper;
    }
    public async Task<List<RecipeResult>> SearchRecipe(SearchRecipeResultsVm searchRecipeResultsVm)
    {
        var list = await _recipesSearchService
            .GetByStatus(searchRecipeResultsVm.StatusId);
        var recipeResult = GetRecipeResults(list);
        return recipeResult;
    }
    private List<RecipeResult> GetRecipeResults(List<Recipe> recipesList)
    {
        List<RecipeResult> recipeResultsList = new List<RecipeResult>();
        foreach (var item in recipesList)
        {
            RecipeResult recipeResult = _mapper.Map<RecipeResult>(item);
            recipeResultsList.Add(recipeResult);
        }
        return recipeResultsList;
    }
    public SearchType GetSearchType(SearchRecipeResultsVm searchRecipeResultsVm)
    {
        return SearchType.Admin;
    }
}