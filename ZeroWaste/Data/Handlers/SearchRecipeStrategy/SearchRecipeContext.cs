using AutoMapper;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.Services.RecipesSearch;
using ZeroWaste.Data.Structs;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.RecipeSearch;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers.SearchRecipeStrategy;

public class SearchRecipeContext : ISearchRecipeContext
{
    private ISearchRecipeStrategy _recipeStrategy;
    private readonly IRecipesSearchService _recipesSearchService;
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;
    public SearchRecipeContext(IRecipesSearchService recipesSearchService, ICategoryService categoryService, IMapper mapper)
    {
        _recipesSearchService = recipesSearchService;
        _categoryService = categoryService;
        _mapper = mapper;
    }
    public async Task<SearchRecipeResultsVm> GetSearchByIngredientsVm(SearchRecipeResultsVm recipeResultsVm)
    {
        SetRecipeStrategy(recipeResultsVm);
        recipeResultsVm.RecipesList = await _recipeStrategy.SearchRecipe(recipeResultsVm);
        recipeResultsVm.CategoryList = await _categoryService.GetAllAsync();
        return recipeResultsVm;
    }
    private void SetRecipeStrategy(SearchRecipeResultsVm recipeResultsVm)
    {
        if ((!IsRecipeListNullOrEmpty(recipeResultsVm.IngredientsLists)) && recipeResultsVm.CategoryId == 0)
            _recipeStrategy = new SearchByIngredientsStrategy(_recipesSearchService, _mapper);
        else if (IsRecipeListNullOrEmpty(recipeResultsVm.IngredientsLists) && recipeResultsVm.CategoryId != 0)
            _recipeStrategy = new SearchByCategoryStrategy(_recipesSearchService, _mapper);
        else if ((!IsRecipeListNullOrEmpty(recipeResultsVm.IngredientsLists)) && recipeResultsVm.CategoryId != 0)
            _recipeStrategy = new SearchByCategoryStrategy(_recipesSearchService, _mapper);
    }
    private bool IsRecipeListNullOrEmpty(List<IngredientForSearch> list)
    {
        return (list == null || (!list.Any()!));
    }
}