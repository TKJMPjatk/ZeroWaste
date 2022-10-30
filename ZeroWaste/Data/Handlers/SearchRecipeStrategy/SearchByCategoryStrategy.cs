using AutoMapper;
using ZeroWaste.Data.Enums;
using ZeroWaste.Data.Services.RecipesSearch;
using ZeroWaste.Data.Structs;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers.SearchRecipeStrategy;

public class SearchByCategoryStrategy : ISearchRecipeStrategy
{    private readonly IRecipesSearchService _recipesSearchService;
    private readonly IMapper _mapper;
    public SearchByCategoryStrategy(IRecipesSearchService recipesSearchService, IMapper mapper)
    {
        _recipesSearchService = recipesSearchService;
        _mapper = mapper;
    }
    public async Task<List<RecipeResult>> SearchRecipe(SearchRecipeResultsVm searchRecipeResultsVm)
    {
        var recipeList = await _recipesSearchService.GetByCategoryAsync(searchRecipeResultsVm.CategoryId);
        var recipeResultList = GetRecipeResultsList(recipeList);
        return recipeResultList;
    }

    public SearchType GetSearchType(SearchRecipeResultsVm searchRecipeResultsVm)
    {
        return SearchType.Categories;
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