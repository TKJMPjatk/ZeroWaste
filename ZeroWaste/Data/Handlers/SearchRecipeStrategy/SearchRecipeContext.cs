using AutoMapper;
using ZeroWaste.Data.Enums;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.Services.Photo;
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
    private readonly IPhotoService _photoService;
    public SearchRecipeContext(IRecipesSearchService recipesSearchService, ICategoryService categoryService, IPhotoService photoService,
        IMapper mapper)
    {
        _recipesSearchService = recipesSearchService;
        _categoryService = categoryService;
        _photoService = photoService;
        _mapper = mapper;
    }


    public async Task<SearchRecipeResultsVm> GetSearchRecipeResultVm(SearchRecipeResultsVm recipeResultsVm)
    {
        SetRecipeStrategy(recipeResultsVm);
        recipeResultsVm.RecipesList = await _recipeStrategy.SearchRecipe(recipeResultsVm);
        recipeResultsVm.CategoryList = await _categoryService.GetAllAsync();
        recipeResultsVm.SearchType = _recipeStrategy.GetSearchType(recipeResultsVm);
        await FillRecipesWithPhotos(recipeResultsVm.RecipesList);
        return recipeResultsVm;
    }
    private async Task FillRecipesWithPhotos(List<RecipeResult> recipeResults)
    {
        foreach (var recipe in recipeResults)
        {
            recipe.Photo =  await _photoService.GetFirstByRecipeAsync(recipe.Id);
        }
    }
    private void SetRecipeStrategy(SearchRecipeResultsVm recipeResultsVm)
    {
        if (recipeResultsVm.SearchType == SearchType.Ingredients)
            _recipeStrategy = new SearchByIngredientsStrategy(_recipesSearchService, _mapper);
        else if (recipeResultsVm.SearchType == SearchType.Categories)
            _recipeStrategy = new SearchByCategoryStrategy(_recipesSearchService, _mapper);
        else if (recipeResultsVm.SearchType == SearchType.IngredientsFiltered)
            _recipeStrategy = new SearchByAllStrategy(_recipesSearchService, _mapper);
        else if (recipeResultsVm.SearchType == SearchType.Admin)
            _recipeStrategy = new SearchForConfirm(_recipesSearchService, _mapper);
        else if (recipeResultsVm.SearchType == SearchType.Favourite)
            _recipeStrategy = new SearchFavourite(_recipesSearchService, _mapper);
        else
            _recipeStrategy = new SearchByIngredientsStrategy(_recipesSearchService, _mapper);
    }
    private bool IsRecipeListNullOrEmpty(List<IngredientForSearch> list)
    {
        //Todo: przeniesienie tego do jakieś metody statycznej albo extensions methods
        return (list == null || (!list.Any()!));
    }
}