using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ZeroWaste.Data;
using ZeroWaste.Data.Enums;
using ZeroWaste.Data.Handlers.SearchRecipesHandlers;
using ZeroWaste.Data.Helpers;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.Services.RecipesSearch;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.Recipes;
using ZeroWaste.Models;

namespace ZeroWaste.Controllers;

public class SearchRecipesController : Controller
{
    private readonly IUrlQueryHelper _urlQueryHelper;
    private readonly ICategoryService _categoryService;
    private readonly ISearchRecipeHandler _searchRecipeHandler;
    private readonly IRecipesSearchService _recipesSearchService;
    private readonly AppDbContext _context;
    public SearchRecipesController(AppDbContext context, IUrlQueryHelper urlQueryHelper, ICategoryService categoryService, ISearchRecipeHandler searchRecipeHandler, IRecipesSearchService recipesSearchService)
    {
        _context = context;
        _urlQueryHelper = urlQueryHelper;
        _categoryService = categoryService;
        _searchRecipeHandler = searchRecipeHandler;
        _recipesSearchService = recipesSearchService;
    }
    [HttpGet]
    public async Task<JsonResult> SearchByIngredientsAuto()
    {
        string term = HttpContext.Request.Query["term"].ToString();
        var list = await _context.Ingredients.Where( x=> x.Name.StartsWith(term)).Select(x => x.Name).ToListAsync();
        return Json(list);
    }
    public IActionResult SearchByIngredients()
    {
        return View();
    }
    public async Task<IActionResult> SearchByIngredientsResult(string ingredientsVm)
    {
        ViewBag.PageTitle = "Wyszukiwanie po składnikach";
        ViewBag.SortTypes = Enum.GetNames(typeof(SortTypes)).ToList();
        //Tworzenie listy ze składnikami
        List<IngredientsForSearchVM> ingredientsForSearchList = new List<IngredientsForSearchVM>();
        if(!(string.IsNullOrEmpty(ingredientsVm)))
            ingredientsForSearchList = _urlQueryHelper.GetIngredientsFromUrl(ingredientsVm);
        //Wyszukanie przepisów - stworzenie SearchRecipeResultVM
        var searchRecipeVm = await _recipesSearchService.GetByIngredients(ingredientsForSearchList);
        searchRecipeVm.CategoryList = await _categoryService.GetAllAsync();
        searchRecipeVm.IngredientForSearchList = ingredientsForSearchList;
        return View(searchRecipeVm);
    }
    [HttpPost]
    public async Task<IActionResult> SearchByIngredientsFilteredResult(SearchRecipeResultsVm resultsVm)
    {
        ViewBag.PageTitle = resultsVm.PageTitle;
        ViewBag.SortTypes = Enum.GetNames(typeof(SortTypes)).ToList();
        List<IngredientsForSearchVM> ingredientsForSearchList = new List<IngredientsForSearchVM>();
        var searchRecipeVm = await _recipesSearchService.GetByIngredients(ingredientsForSearchList);
        return View(nameof(SearchByIngredientsResult), searchRecipeVm);
    }
    public async Task<IActionResult> SearchByCategories()
    {
        List<Category> categories = await _categoryService.GetAllAsync();
        var tmp = await _searchRecipeHandler.GetCategoriesSearchVm();
        return View(tmp);
    }
    public async Task<IActionResult> SearchByCategoryResult(int categoryId)
    {
        var list = await _recipesSearchService.GetByCategoryAsync(categoryId);
        ViewBag.PageTitle = "Wyszukiwanie po kategoriach";
        return View("SearchByIngredientsResult");
    }
}