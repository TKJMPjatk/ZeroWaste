using System.Security.Claims;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ZeroWaste.Data;
using ZeroWaste.Data.Enums;
using ZeroWaste.Data.Handlers.SearchRecipesHandlers;
using ZeroWaste.Data.Helpers;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.Services.RecipesSearch;
using ZeroWaste.Data.Services.Statuses;
using ZeroWaste.Data.Static;
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
    private readonly IStatusesService _statusesService;
    private readonly AppDbContext _context;
    public SearchRecipesController(AppDbContext context, ICategoryService categoryService, ISearchRecipeHandler searchRecipeHandler, IStatusesService statusesService)
    {
        _context = context;
        _categoryService = categoryService;
        _statusesService = statusesService;
        _searchRecipeHandler = searchRecipeHandler;
    }
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult SearchByIngredients()
    {
        return View(new SearchByIngredientsVm());
    }
    [Microsoft.AspNetCore.Mvc.HttpPost]
    public IActionResult AddIngredient(SearchByIngredientsVm searchByIngredientsVm)
    {
        if (!(ModelState.IsValid))
        {
            return View("SearchByIngredients", searchByIngredientsVm);
        }
        var newSearchByIngredientsVm = _searchRecipeHandler.AddIngredient(searchByIngredientsVm);
        return View("SearchByIngredients", newSearchByIngredientsVm);
    }
    [Microsoft.AspNetCore.Mvc.HttpGet]
    public async Task<JsonResult> SearchByIngredientsAuto()
    {
        string term = HttpContext.Request.Query["term"].ToString();
        var list = await _context.Ingredients.Where( x=> x.Name.StartsWith(term)).Select(x => x.Name).ToListAsync();
        return Json(list);
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
    [Microsoft.AspNetCore.Mvc.HttpPost]
    public async Task<IActionResult> SearchByIngredientsResult(SearchByIngredientsVm searchByIngredientsVm)
    {
        ViewBag.PageTitle = "Wyszukiwanie po składnikach";
        ViewBag.SortTypes = Enum.GetValues(typeof(SortTypes)).Cast<SortTypes>().ToList();
        SearchRecipeResultsVm searchRecipeResultsVm =
            await _searchRecipeHandler.GetSearchRecipeResultVmByIngredients(searchByIngredientsVm);
        return View("SearchResult", searchRecipeResultsVm);
    }
    [Microsoft.AspNetCore.Mvc.HttpPost]
    public async Task<IActionResult> SearchRecipesFilteredResult(SearchRecipeResultsVm resultsVm)
    {
        ViewBag.PageTitle = resultsVm.PageTitle;
        ViewBag.SortTypes = Enum.GetValues(typeof(SortTypes)).Cast<SortTypes>().ToList();
        resultsVm = await _searchRecipeHandler.GetSearchRecipeResultVmFiltered(resultsVm);
        if (resultsVm.SortTypeId != 0)
        {
            var result = await _searchRecipeHandler.GetSearchRecipeResultVmSorted(resultsVm);
        }
        return View("SearchResult", resultsVm);
    }

    [Microsoft.AspNetCore.Mvc.HttpPost]
    public async Task<IActionResult> SearchRecipeSentenceResult(SearchRecipeResultsVm resultsVm)
    {
        ViewBag.PageTitle = resultsVm.PageTitle;
        ViewBag.SortTypes = Enum.GetValues(typeof(SortTypes)).Cast<SortTypes>().ToList();
        if(resultsVm.RecipesListBase.IsRecipeResultNullOrEmpty())
            resultsVm.RecipesListBase = resultsVm.RecipesList;
        if (string.IsNullOrEmpty(resultsVm.SearchSentence))
            resultsVm.RecipesList = resultsVm.RecipesListBase;
        else
            resultsVm.RecipesList = resultsVm.RecipesListBase.Where(x =>
                    x.Title.Contains(resultsVm.SearchSentence))
                .ToList();
        return View("SearchResult", resultsVm);
    }
    public async Task<IActionResult> SearchRecipesSortedResult(SearchRecipeResultsVm resultsVm)
    {
        ViewBag.PageTitle = resultsVm.PageTitle;
        ViewBag.SortTypes = Enum.GetValues(typeof(SortTypes)).Cast<SortTypes>().ToList();
        var result = await _searchRecipeHandler.GetSearchRecipeResultVmSorted(resultsVm);
        return View("SearchResult", resultsVm);
    }
    [Microsoft.AspNetCore.Mvc.HttpPost]
    public async Task<IActionResult> ReturnToSearchByIngredients(SearchRecipeResultsVm resultsVm)
    {
        SearchByIngredientsVm searchByIngredientsVm = new SearchByIngredientsVm()
        {
            SingleIngredientToSearchVm = resultsVm.IngredientsLists
        };
        return View("SearchByIngredients", searchByIngredientsVm);
    }
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> SearchForConfirm(int statusId = 1)
    {
        ViewBag.SortTypes = Enum.GetValues(typeof(SortTypes)).Cast<SortTypes>().ToList();
        ViewBag.Statuses = await _statusesService.GetAllAsync();
        ViewBag.PageTitle = "Przepisy do zatwierdzenia";
        var resultsVm = await _searchRecipeHandler.GetSearchRecipeResultVmForConfirm(statusId);
        return View("SearchResult", resultsVm);
    }

    public async Task<IActionResult> SearchFavourite()
    {
        ViewBag.SortTypes = Enum.GetValues(typeof(SortTypes)).Cast<SortTypes>().ToList();
        ViewBag.Statuses = await _statusesService.GetAllAsync();
        ViewBag.PageTitle = "Ulubione przepisy";
        var resultsVm = await _searchRecipeHandler
                .GetSearchRecipeResultVmFavourite(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        return View("SearchResult", resultsVm);
    }
}