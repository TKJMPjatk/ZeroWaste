using System.Linq.Dynamic.Core;
using AutoMapper;
using ZeroWaste.Data.Enums;
using ZeroWaste.Data.Handlers.SearchRecipeStrategy;
using ZeroWaste.Data.Helpers.RecipeCategoryImage;
using ZeroWaste.Data.Helpers.SearchByIngredients;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.Services.Photo;
using ZeroWaste.Data.Services.Recipes;
using ZeroWaste.Data.Services.RecipesSearch;
using ZeroWaste.Data.Structs;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.CategorySearch;
using ZeroWaste.Data.ViewModels.RecipeSearch;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers.SearchRecipesHandlers;

public class SearchRecipeHandler : ISearchRecipeHandler
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;
    private IWebHostEnvironment _hostEnvironment;
    private readonly ISearchRecipeContext _searchRecipeContext;
    private readonly IPhotoService _photoService;
    private readonly IRecipesService _recipesService;

    private readonly ICategorySearchVmMapper _categorySearchVmMapper;
    private readonly ISearchByIngredientAdder _searchByIngredientAdder; 
    
    public SearchRecipeHandler(ISearchRecipeContext searchRecipeContext, ICategoryService categoryService, IMapper mapper, IWebHostEnvironment hostEnvironment, IPhotoService photoService, IRecipesService recipesService, ICategorySearchVmMapper categorySearchVmMapper, ISearchByIngredientAdder searchByIngredientAdder)
    {
        _categoryService = categoryService;
        _mapper = mapper;
        _hostEnvironment = hostEnvironment;
        _searchRecipeContext = searchRecipeContext;
        _photoService = photoService;
        _recipesService = recipesService;
        
        _categorySearchVmMapper = categorySearchVmMapper;
        _searchByIngredientAdder = searchByIngredientAdder;
    }
    public async Task<List<CategorySearchVm>> GetCategoriesSearchVm()
    {
        List<Category> categories = await _categoryService.GetAllAsync();
        return _categorySearchVmMapper.MapToCategorySearchVmsList(categories);
    }
    public SearchByIngredientsVm AddIngredient(SearchByIngredientsVm searchByIngredientsVm)
    {
        _searchByIngredientAdder.AddIngredientToSearchByIngredientsVm(searchByIngredientsVm);
        return searchByIngredientsVm;
    }

    public SearchByIngredientsVm DeleteIngredient(SearchByIngredientsVm searchByIngredientsVm)
    {
        var item = searchByIngredientsVm.SingleIngredientToSearchVm.FirstOrDefault(x =>
            x.Name == searchByIngredientsVm.Name);
        searchByIngredientsVm.SingleIngredientToSearchVm.Remove(item);
        searchByIngredientsVm.Name = string.Empty;
        for (int i = 0; i < searchByIngredientsVm.SingleIngredientToSearchVm.Count; i++)
        {
            searchByIngredientsVm.SingleIngredientToSearchVm[i].Index = i + 1;
        }
        return searchByIngredientsVm;
    }

    public async Task<SearchRecipeResultsVm> GetSearchRecipeResultVmSorted(SearchRecipeResultsVm resultsVm)
    {
        resultsVm.RecipesList = GetSortField(resultsVm.RecipesList, resultsVm.SortTypeId);
        await FillRecipesWithPhotos(resultsVm.RecipesList);
        return resultsVm;
    }
    public async Task<int> GetRandomRecipeId()
    {
        List<int> recipeIdsList = await _recipesService.GetRecipeIdList();
        Random r = new();
        int randomNumber = r.Next(0, recipeIdsList.Count - 1);
        return recipeIdsList[randomNumber];
    }
    private List<RecipeResult> GetSortField(List<RecipeResult> recipeResults, int sortTypeId)
    {
        if (sortTypeId == (int) SortTypes.ByTime)
            return recipeResults.OrderBy(x => x.EstimatedTime).ToList();
        if (sortTypeId== (int) SortTypes.ByDifficultyLevel) 
            return recipeResults.OrderBy(x => x.DifficultyLevel).ToList();
        if (sortTypeId == (int) SortTypes.FromAtoZ)
            return recipeResults.OrderBy(x => x.Title).ToList();
        if (sortTypeId == (int) SortTypes.FromZToA)
            return recipeResults.OrderByDescending(x => x.Title).ToList();
        if (sortTypeId == (int) SortTypes.ByGrades)
            return recipeResults.OrderByDescending(x => x.Stars).ToList();
        return recipeResults;
    }
    private async Task FillRecipesWithPhotos(List<RecipeResult> recipeResults)
    {
        foreach (var recipe in recipeResults)
        {
            recipe.Photo =  await _photoService.GetFirstByRecipeAsync(recipe.Id);
        }
    }
}