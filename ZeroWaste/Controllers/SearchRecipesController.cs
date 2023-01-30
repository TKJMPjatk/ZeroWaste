using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZeroWaste.Data;
using ZeroWaste.Data.Enums;
using ZeroWaste.Data.Handlers.SearchRecipesHandlers;
using ZeroWaste.Data.Handlers.SearchRecipeStrategy;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.Services.RecipeIngredients;
using ZeroWaste.Data.Services.Statuses;
using ZeroWaste.Data.Static;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.RecipeSearch;
using ZeroWaste.Models;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using JsonResult = Microsoft.AspNetCore.Mvc.JsonResult;

namespace ZeroWaste.Controllers;

public class SearchRecipesController : Controller
{
    private List<SortTypeDisplayVm> _sortTypeDisplayVmList;
    private readonly ICategoryService _categoryService;
    private readonly ISearchRecipeHandler _searchRecipeHandler;
    private readonly IStatusesService _statusesService;
    private readonly ISearchRecipeContext _searchRecipeContext;
    private readonly IRecipeIngredientService _recipeIngredientService;
    private readonly AppDbContext _context;
    public SearchRecipesController(AppDbContext context, ICategoryService categoryService, ISearchRecipeHandler searchRecipeHandler, IStatusesService statusesService, ISearchRecipeContext searchRecipeContext, IRecipeIngredientService service)
    {
        _context = context;
        _categoryService = categoryService;
        _statusesService = statusesService;
        _searchRecipeHandler = searchRecipeHandler;
        _searchRecipeContext = searchRecipeContext;
        _sortTypeDisplayVmList = SortTypeSupplementer.SupplementSortTypeVm();
        _recipeIngredientService = service;
    }
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> SupriseMeResult()
    {
        string userId = null;
        if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
        {
            userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        int recipeId = await _searchRecipeHandler.GetRandomRecipeId(userId);
        return RedirectToAction("Details", "Recipes", new {id = recipeId});
    }
    public async Task<IActionResult> SearchByIngredientsAsync()
    {
        string userId = null;
        if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
        {
            userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        var recipeIngredientsDropdownsData = await _recipeIngredientService.GetDropdownIngredientsAsync(userId);
        ViewBag.Ingredients = recipeIngredientsDropdownsData.ToList();
        return View(new SearchByIngredientsVm());
    }
    [HttpPost]
    public async Task<IActionResult> AddIngredientAsync(SearchByIngredientsVm searchByIngredientsVm)
    {
        string userId = null;
        if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
        {
            userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        var recipeIngredientsDropdownsData = await _recipeIngredientService.GetDropdownIngredientsAsync(userId);
        ViewBag.Ingredients = recipeIngredientsDropdownsData.ToList();
        if (!(ModelState.IsValid))
        {
            return View("SearchByIngredients", searchByIngredientsVm);
        }
        var newSearchByIngredientsVm = _searchRecipeHandler.AddIngredient(searchByIngredientsVm);
        return View("SearchByIngredients", newSearchByIngredientsVm);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteIngredient(SearchByIngredientsVm searchByIngredientsVm)
    {
        string userId = null;
        if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
        {
            userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        var recipeIngredientsDropdownsData = await _recipeIngredientService.GetDropdownIngredientsAsync(userId);
        ViewBag.Ingredients = recipeIngredientsDropdownsData.ToList();
        var searchByIngredientsResult = _searchRecipeHandler.DeleteIngredient(searchByIngredientsVm);
        ModelState.Clear();
        return View("SearchByIngredients", searchByIngredientsResult);
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
        //ViewBag.SortTypes = Enum.GetValues(typeof(SortTypes)).Cast<SortTypes>().ToList();
        ViewBag.SortTypes = _sortTypeDisplayVmList;
        ViewBag.PageTitle = "Wyszukiwanie po kategoriach";
        var user = User.FindFirst(ClaimTypes.NameIdentifier);
        string userId = null;
        if (user is not null)
        {
            userId = user.Value;
        }
        var resultVm = await _searchRecipeContext.GetSearchRecipeResultVm(new SearchRecipeResultsVm()
        {
            CategoryId = categoryId,
            SearchType = SearchType.Categories,
            UserId = userId//User.FindFirst(ClaimTypes.NameIdentifier).Value,
        });
        return View("SearchResult", resultVm);
    }
    [HttpPost]
    public async Task<IActionResult> SearchByIngredientsResult(SearchByIngredientsVm searchByIngredientsVm)
    {
        ViewBag.PageTitle = "Wyszukiwanie po składnikach";
        ViewBag.SortTypes = _sortTypeDisplayVmList;
        //ViewBag.SortTypes = Enum.GetValues(typeof(SortTypes)).Cast<SortTypes>().ToList();
        var user = User.FindFirst(ClaimTypes.NameIdentifier);
        string userId = null;
        if (user is not null)
        {
            userId = user.Value;
        }
        SearchRecipeResultsVm searchRecipeResultsVm =
            await _searchRecipeContext.GetSearchRecipeResultVm(new SearchRecipeResultsVm()
            {
                IngredientsLists = searchByIngredientsVm.SingleIngredientToSearchVm,
                SearchType = SearchType.Ingredients,
                UserId = userId
            });
        return View("SearchResult", searchRecipeResultsVm);
    }
    [HttpPost]
    public async Task<IActionResult> SearchRecipesFilteredResult(SearchRecipeResultsVm resultsVm)
    {
        ViewBag.PageTitle = resultsVm.PageTitle;
        ViewBag.SortTypes = _sortTypeDisplayVmList;
        var user = User.FindFirst(ClaimTypes.NameIdentifier);
        string userId = null;
        if (user is not null)
        {
            userId = user.Value;
        }
        if(resultsVm.IngredientsLists != null && resultsVm.SearchType != SearchType.Categories)
        {
            resultsVm.SearchType = SearchType.IngredientsFiltered;
            resultsVm.UserId = userId;
            resultsVm = await _searchRecipeContext.GetSearchRecipeResultVm(resultsVm);
        }
        else
        {
            resultsVm.SearchType = SearchType.Categories;
            resultsVm.UserId = userId;
            resultsVm = await _searchRecipeContext.GetSearchRecipeResultVm(resultsVm);
        }
        return View("SearchResult", resultsVm);
    }

    [HttpPost]
    public IActionResult SearchRecipeSentenceResult(SearchRecipeResultsVm resultsVm)
    {
        ViewBag.PageTitle = resultsVm.PageTitle;
        ViewBag.SortTypes = _sortTypeDisplayVmList;
        if (resultsVm.RecipesListBase.IsRecipeResultNullOrEmpty())
            resultsVm.RecipesListBase = resultsVm.RecipesList;
        if (string.IsNullOrEmpty(resultsVm.SearchSentence))
            resultsVm.RecipesList = resultsVm.RecipesListBase;
        else
            resultsVm.RecipesList = resultsVm.RecipesListBase.Where(x =>
                    x.Title.ToLower().Contains(resultsVm.SearchSentence.ToLower()))
                .ToList();
        return View("SearchResult", resultsVm);
    }
    public async Task<IActionResult> SearchRecipesSortedResult(SearchRecipeResultsVm resultsVm)
    {
        ViewBag.PageTitle = resultsVm.PageTitle;
        ViewBag.SortTypes = _sortTypeDisplayVmList;
        var result = await _searchRecipeHandler.GetSearchRecipeResultVmSorted(resultsVm);
        return View("SearchResult", resultsVm);
    }
    [HttpPost]
    public async Task<IActionResult> ReturnToSearchByIngredients(SearchRecipeResultsVm resultsVm)
    {
        string userId = null;
        if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
        {
            userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        var recipeIngredientsDropdownsData = await _recipeIngredientService.GetDropdownIngredientsAsync(userId);
        ViewBag.Ingredients = recipeIngredientsDropdownsData.ToList();
        SearchByIngredientsVm searchByIngredientsVm = new()
        {
            SingleIngredientToSearchVm = resultsVm.IngredientsLists
        };
        return View("SearchByIngredients", searchByIngredientsVm);
    }
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> SearchForConfirm(int statusId = 2)
    {
        //ViewBag.SortTypes = Enum.GetValues(typeof(SortTypes)).Cast<SortTypes>().ToList();
        ViewBag.SortTypes = _sortTypeDisplayVmList;
        ViewBag.Statuses = await _statusesService.GetAllAsync();
        ViewBag.PageTitle = "Przepisy do zatwierdzenia";
        var resultsVm = await _searchRecipeContext.GetSearchRecipeResultVm(new SearchRecipeResultsVm()
        {
            StatusId = statusId,
            SearchType = SearchType.Admin
        });
        return View("SearchResult", resultsVm);
    }
    public async Task<IActionResult> SearchFavourite()
    {
        //ViewBag.SortTypes = Enum.GetValues(typeof(SortTypes)).Cast<SortTypes>().ToList();
        ViewBag.SortTypes = _sortTypeDisplayVmList;
        ViewBag.Statuses = await _statusesService.GetAllAsync();
        ViewBag.PageTitle = "Ulubione przepisy";
        var resultsVm = await _searchRecipeContext.GetSearchRecipeResultVm(new SearchRecipeResultsVm()
                {
                    UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    SearchType = SearchType.Favourite
                });
        return View("SearchResult", resultsVm);
    }
    public async Task<IActionResult> SearchHated()
    {
        //ViewBag.SortTypes = Enum.GetValues(typeof(SortTypes)).Cast<SortTypes>().ToList();
        ViewBag.SortTypes = _sortTypeDisplayVmList;
        ViewBag.Statuses = await _statusesService.GetAllAsync();
        ViewBag.PageTitle = "Nielubiane przepisy";
        var resultsVm = await _searchRecipeContext.GetSearchRecipeResultVm(new SearchRecipeResultsVm()
        {
            UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
            SearchType = SearchType.Hated
        });
        return View("SearchResult", resultsVm);
    }
    public async Task<IActionResult> SearchMineToEdit(string? success)
    {
        //ViewBag.SortTypes = Enum.GetValues(typeof(SortTypes)).Cast<SortTypes>().ToList();
        ViewBag.SortTypes = _sortTypeDisplayVmList;
        ViewBag.Statuses = await _statusesService.GetAllAsync();
        ViewBag.PageTitle = "Moje przepisy";
        ViewBag.Success = success;
        var resultVm = await _searchRecipeContext.GetSearchRecipeResultVm(new SearchRecipeResultsVm()
        {
            UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
            SearchType = SearchType.EditMine
        });
        return View("SearchResult", resultVm);
    }
}