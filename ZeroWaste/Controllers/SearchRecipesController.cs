using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.Handlers.SearchRecipesHandlers;
using ZeroWaste.Data.Helpers;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.Services.RecipesSearch;
using ZeroWaste.Data.ViewModels.Recipes;
using ZeroWaste.Models;

namespace ZeroWaste.Controllers;

public class SearchRecipesController : Controller
{
    private readonly IUrlQueryHelper _urlQueryHelper;
    private readonly ICategoryService _categoryService;
    private readonly ISearchRecipeHandler _searchRecipeHandler;
    private readonly IRecipesSearchService _recipesSearchService;
    public SearchRecipesController(IUrlQueryHelper urlQueryHelper, ICategoryService categoryService, ISearchRecipeHandler searchRecipeHandler, IRecipesSearchService recipesSearchService)
    {
        _urlQueryHelper = urlQueryHelper;
        _categoryService = categoryService;
        _searchRecipeHandler = searchRecipeHandler;
        _recipesSearchService = recipesSearchService;
    }   
    public IActionResult SearchByIngredients()
    {
        return View();
    }
    public IActionResult SearchByIngredientsResult(string ingredientsVm)
    {
        List<IngredientsForSearchVM> ingredientsForSearchList;
        if(ingredientsVm != null)
            ingredientsForSearchList = _urlQueryHelper.GetIngredientsFromUrl(ingredientsVm);
        ViewBag.Title = "Wyszukiwanie po składnikach";
        return View();
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
        ViewBag.Title = "Wyszukiwanie po kategoriach";
        return View("SearchByIngredientsResult", list);
    }
}