using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.Helpers;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.ViewModels.Recipes;
using ZeroWaste.Models;

namespace ZeroWaste.Controllers;

public class SearchRecipesController : Controller
{
    private readonly IUrlQueryHelper _urlQueryHelper;
    private readonly ICategoryService _categoryService;
    public SearchRecipesController(IUrlQueryHelper urlQueryHelper, ICategoryService categoryService)
    {
        _urlQueryHelper = urlQueryHelper;
        _categoryService = categoryService;
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
        return View();
    }
    public async Task<IActionResult> SearchByCategories()
    {
        List<Category> categories = await _categoryService.GetAllAsync();
        return View(categories);
    }
}