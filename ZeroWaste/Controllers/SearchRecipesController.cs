using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.Helpers;
using ZeroWaste.Data.ViewModels.Recipes;

namespace ZeroWaste.Controllers;

public class SearchRecipesController : Controller
{
    private readonly IUrlQueryHelper _urlQueryHelper;
    public SearchRecipesController(IUrlQueryHelper urlQueryHelper)
    {
        _urlQueryHelper = urlQueryHelper;
    }   
    public IActionResult SearchByIngredients()
    {
        return View();
    }
    public IActionResult SearchByIngredientsResult(string ingredientsVm)
    {
        List<IngredientsForSearchVM> ingredientsForSearchList = _urlQueryHelper.GetIngredientsFromUrl(ingredientsVm);
        return View();
    }
}