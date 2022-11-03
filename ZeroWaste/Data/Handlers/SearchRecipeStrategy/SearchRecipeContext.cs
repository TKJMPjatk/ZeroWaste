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
            _recipeStrategy = new SearchByIngredientsStrategy(_recipesSearchService);
        else if (recipeResultsVm.SearchType == SearchType.Categories)
            _recipeStrategy = new SearchByCategoryStrategy(_recipesSearchService);
        else if (recipeResultsVm.SearchType == SearchType.IngredientsFiltered)
            _recipeStrategy = new SearchByAllStrategy(_recipesSearchService);
        else if (recipeResultsVm.SearchType == SearchType.Admin)
            _recipeStrategy = new SearchForConfirm(_recipesSearchService);
        else if (recipeResultsVm.SearchType == SearchType.Favourite)
            _recipeStrategy = new SearchFavourite(_recipesSearchService);
        else if (recipeResultsVm.SearchType == SearchType.Hated)
            _recipeStrategy = new SearchHated(_recipesSearchService);
        else if (recipeResultsVm.SearchType == SearchType.EditMine)
            _recipeStrategy = new SearchMineToEdit(_recipesSearchService);
    }
}