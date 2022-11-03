using System.Linq.Dynamic.Core;
using AutoMapper;
using ZeroWaste.Data.Enums;
using ZeroWaste.Data.Handlers.SearchRecipeStrategy;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.Services.Photo;
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
    public SearchRecipeHandler(ISearchRecipeContext searchRecipeContext, ICategoryService categoryService, IMapper mapper, IWebHostEnvironment hostEnvironment, IPhotoService photoService)
    {
        _categoryService = categoryService;
        _mapper = mapper;
        _hostEnvironment = hostEnvironment;
        _searchRecipeContext = searchRecipeContext;
        _photoService = photoService;
    }
    public async Task<List<CategorySearchVm>> GetCategoriesSearchVm()
    {
        List<Category> categories = await _categoryService.GetAllAsync();
        return MapToCategoriesSearchVms(categories);
    }
    private List<CategorySearchVm> MapToCategoriesSearchVms(List<Category> categories)
    {
        List<CategorySearchVm> categorySearchVmList = new List<CategorySearchVm>();
        foreach (var entity in categories)
        {
            var categorySearchVm = _mapper.Map<CategorySearchVm>(entity);
            categorySearchVm.Image = GetMatchedImage(entity);
            categorySearchVmList.Add(categorySearchVm);
        }
        return categorySearchVmList;
    }
    private string GetMatchedImage(Category category)
    {
        var isFileExist = _hostEnvironment
            .WebRootFileProvider
            .GetFileInfo($"/images/categories/{category.Name}.png")
            .Exists;
        if (isFileExist)
            return $"~/images/categories/{category.Name}.png";
        return $"~/images/categories/Burgery.png";
    }
    public SearchByIngredientsVm AddIngredient(SearchByIngredientsVm searchByIngredientsVm)
    {
        int tmp = searchByIngredientsVm.SingleIngredientToSearchVm.Count;
        searchByIngredientsVm.SingleIngredientToSearchVm.Add(new IngredientForSearch() 
        {
            Name = searchByIngredientsVm.Name, 
            Quantity = searchByIngredientsVm.Quantity, 
            Index = tmp+1,
        }); 
        searchByIngredientsVm.Name = string.Empty; 
        searchByIngredientsVm.Quantity = 0; 
        return searchByIngredientsVm;
    }
    public async Task<SearchRecipeResultsVm> GetSearchRecipeResultVmByIngredients(SearchByIngredientsVm searchByIngredientsVm)
    {
        var results = await _searchRecipeContext
            .GetSearchRecipeResultVm(new SearchRecipeResultsVm()
        {
            IngredientsLists = searchByIngredientsVm.SingleIngredientToSearchVm
        });
        return results;
    }
    public async Task<SearchRecipeResultsVm> GetSearchRecipeResultVmByCategory(int categoryId)
    {
        var results = await _searchRecipeContext
            .GetSearchRecipeResultVm(new SearchRecipeResultsVm()
        {
            CategoryId = categoryId
        });
        await FillRecipesWithPhotos(results.RecipesList);
        return results;
    }
    public async Task<SearchRecipeResultsVm> GetSearchRecipeResultVmForConfirm(int statusId)
    {
        var results = await _searchRecipeContext.GetSearchRecipeResultVm(new SearchRecipeResultsVm()
        {
            StatusId = statusId
        });
        await FillRecipesWithPhotos(results.RecipesList);
        return results;
    }
    public async Task<SearchRecipeResultsVm> GetSearchRecipeResultVmFavourite(string userId)
    {
        var results = await _searchRecipeContext.GetSearchRecipeResultVm(new SearchRecipeResultsVm()
        {
            UserId = userId
        });
        await FillRecipesWithPhotos(results.RecipesList);
        return results;
    }
    public async Task<SearchRecipeResultsVm> GetSearchRecipeResultVmFiltered(SearchRecipeResultsVm searchRecipeResultsVm)
    {
        var results = await _searchRecipeContext
            .GetSearchRecipeResultVm(searchRecipeResultsVm);
        await FillRecipesWithPhotos(results.RecipesList);
        return results;
    }
    public async Task<SearchRecipeResultsVm> GetSearchRecipeResultVmSorted(SearchRecipeResultsVm resultsVm)
    {
        resultsVm.RecipesList = GetSortField(resultsVm.RecipesList, resultsVm.SortTypeId);
        await FillRecipesWithPhotos(resultsVm.RecipesList);
        return resultsVm;
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