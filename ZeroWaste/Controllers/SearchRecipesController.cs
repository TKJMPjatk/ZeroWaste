using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ZeroWaste.Data;
using ZeroWaste.Data.Enums;
using ZeroWaste.Data.Handlers.SearchRecipesHandlers;
using ZeroWaste.Data.Helpers;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.Services.RecipesSearch;
using ZeroWaste.Data.Structs;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.Recipes;
using ZeroWaste.Data.ViewModels.RecipeSearch;
using ZeroWaste.Models;

namespace ZeroWaste.Controllers;

public class SearchRecipesController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly ISearchRecipeHandler _searchRecipeHandler;
    private readonly IRecipesSearchService _recipesSearchService;
    private readonly AppDbContext _context;
    public SearchRecipesController(AppDbContext context, ICategoryService categoryService, ISearchRecipeHandler searchRecipeHandler, IRecipesSearchService recipesSearchService)
    {
        _context = context;
        _categoryService = categoryService;
        _searchRecipeHandler = searchRecipeHandler;
        _recipesSearchService = recipesSearchService;
    }
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult SearchByIngredients()
    {
        return View(new SearchByIngredientsVm());
    }
    [HttpPost]
    public IActionResult AddIngredient(SearchByIngredientsVm searchByIngredientsVm)
    {
        if (!(ModelState.IsValid))
        {
            return View("SearchByIngredients", searchByIngredientsVm);
        }
        var newSearchByIngredientsVm = _searchRecipeHandler.AddIngredient(searchByIngredientsVm);
        return View("SearchByIngredients", newSearchByIngredientsVm);
    }
    [HttpGet]
    public async Task<JsonResult> SearchByIngredientsAuto()
    {
        string term = HttpContext.Request.Query["term"].ToString();
        var list = await _context.Ingredients.Where( x=> x.Name.StartsWith(term)).Select(x => x.Name).ToListAsync();
        return Json(list);
    }
    [HttpPost]
    public async Task<IActionResult> SearchResult(SearchByIngredientsVm searchByIngredientsVm)
    {
        ViewBag.PageTitle = "Wyszukiwanie po składnikach";
        ViewBag.SortTypes = Enum.GetValues(typeof(SortTypes)).Cast<SortTypes>().ToList();
        SearchRecipeResultsVm searchRecipeResultsVm =
            await _searchRecipeHandler.GetSearchRecipeResultVm(searchByIngredientsVm);
        return View(searchRecipeResultsVm);
    }
    [HttpPost]
    public async Task<IActionResult> SearchByIngredientsFilteredResult(SearchRecipeResultsVm resultsVm)
    {
        ViewBag.PageTitle = resultsVm.PageTitle;
        ViewBag.SortTypes = Enum.GetValues(typeof(SortTypes)).Cast<SortTypes>().ToList();
        resultsVm = await _searchRecipeHandler.GetSearchRecipeResultVmFiltered(resultsVm);
        //Sortowanie
        if (resultsVm.SortTypeId != 0)
        {
            var result = await _searchRecipeHandler.GetSearchRecipeResultVmSorted(resultsVm);
        }
        return View("SearchResult", resultsVm);
    }

    public async Task<IActionResult> SearchRecipeSortedResult(SearchRecipeResultsVm resultsVm)
    {
        ViewBag.PageTitle = resultsVm.PageTitle;
        ViewBag.SortTypes = Enum.GetValues(typeof(SortTypes)).Cast<SortTypes>().ToList();
        var result = await _searchRecipeHandler.GetSearchRecipeResultVmSorted(resultsVm);
        return View("SearchResult", resultsVm);
    }
    [HttpPost]
    public async Task<IActionResult> ReturnToSearchByIngredients(SearchRecipeResultsVm resultsVm)
    {
        SearchByIngredientsVm searchByIngredientsVm = new SearchByIngredientsVm()
        {
            SingleIngredientToSearchVm = resultsVm.IngredientsLists
        };
        return View("SearchByIngredients", searchByIngredientsVm);
    }
    public async Task<IActionResult> SearchByCategories()
    {
        List<Category> categories = await _categoryService.GetAllAsync();
        var tmp = await _searchRecipeHandler.GetCategoriesSearchVm();
        return View(tmp);
    }
    public async Task<IActionResult> SearchByCategoryResult(int categoryId)
    {
        ViewBag.SortTypes = Enum.GetValues(typeof(SortTypes)).Cast<SortTypes>().ToList();
        ViewBag.PageTitle = "Wyszukiwanie po kategoriach";
        var resultVm = await _searchRecipeHandler.GetSearchRecipeResultVmByCategory(categoryId);
        return View("SearchResult", resultVm);
    }
}