using AutoMapper;
using ZeroWaste.Data.Services;
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
    private readonly IRecipesSearchService _recipesSearchService;
    public SearchRecipeHandler(ICategoryService categoryService, IMapper mapper, IWebHostEnvironment hostEnvironment, IRecipesSearchService recipesSearchService)
    {
        _categoryService = categoryService;
        _mapper = mapper;
        _hostEnvironment = hostEnvironment;
        _recipesSearchService = recipesSearchService;
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
    public async Task<SearchRecipeResultsVm> GetSearchRecipeResultVm(SearchByIngredientsVm searchByIngredientsVm)
    {
        //Inicjalizacja obiektu zwracanego
        SearchRecipeResultsVm searchRecipeResultsVm = new SearchRecipeResultsVm();
        //Przepisy
        var recipeList = await _recipesSearchService.GetByIngredients(searchByIngredientsVm.SingleIngredientToSearchVm);
        List<RecipeResult> recipeResultsList = new List<RecipeResult>();
        foreach (var item in recipeList)
        {
            recipeResultsList.Add(new RecipeResult()
            {
                Id = item.Id,
                Title = item.Title,
                EstimatedTime = item.EstimatedTime,
                DifficultyLevel = item.DifficultyLevel,
                CategoryId = item.CategoryId,
                Ingredients = item.RecipesIngredients.Select(x => x.Ingredient.Name).ToList()
            });
        }
        searchRecipeResultsVm.RecipesList = recipeResultsList;
        //Kategorie
        searchRecipeResultsVm.CategoryList = await _categoryService.GetAllAsync();
        //Lista składników z poprzedniego okna
        searchRecipeResultsVm.IngredientsLists = searchByIngredientsVm.SingleIngredientToSearchVm;
        return searchRecipeResultsVm;
    }
    public async Task<SearchRecipeResultsVm> GetSearchRecipeResultVmByCategory(int categoryId)
    {        
        var list = await _recipesSearchService.GetByCategoryAsync(categoryId);
        List<RecipeResult> recipeResults = new List<RecipeResult>();
        foreach (var recipe in list)
        {
            recipeResults.Add(new RecipeResult()
            {
                Id = recipe.Id,
                Title = recipe.Title,
                CategoryId = recipe.CategoryId,
                DifficultyLevel = recipe.DifficultyLevel,
                EstimatedTime = recipe.EstimatedTime,
                Ingredients = recipe.RecipesIngredients
                    .Select(x => x.Ingredient.Name)
                    .ToList()
            });
        }
        SearchRecipeResultsVm resultsVm = new SearchRecipeResultsVm()
        {
            RecipesList = recipeResults
        };
        resultsVm.CategoryList = await _categoryService.GetAllAsync();
        return resultsVm;
    }
    public async Task<SearchRecipeResultsVm> GetSearchRecipeResultVmFiltered(SearchRecipeResultsVm searchRecipeResultsVm)
    {
        //Wyszukiwanie po liście składników
        List<RecipeResult> recipeResults = new List<RecipeResult>();
        //var recipeList = await _recipesSearchService.GetByIngredients(searchByIngredientsVm);
        //List<RecipeResult> recipeResultsList = new List<RecipeResult>();
        foreach (var item in recipeResults)
        {
            recipeResults.Add(new RecipeResult()
            {
                Id = item.Id,
                Title = item.Title,
                EstimatedTime = item.EstimatedTime,
                DifficultyLevel = item.DifficultyLevel,
                CategoryId = item.CategoryId,
                //Ingredients = item.RecipesIngredients.Select(x => x.Ingredient.Name).ToList()
            });
        }
        if (searchRecipeResultsVm.CategoryId != 0)
        {
            
            
        }
        return searchRecipeResultsVm;
    }
}