using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.ViewModels.Recipes;

namespace ZeroWaste.Controllers;

public class SearchRecipesController : Controller
{
    
    public IActionResult SearchByIngredients()
    {
        return View();
    }
    [HttpPost]
    public IActionResult SearchByIngredients([FromBody]SearchByIngredientsVm ingredientsForSearch)
    {
        return RedirectToAction(nameof(SearchByIngredientsResult));
    }
    public IActionResult SearchByIngredientsResult()
    {
        return View();
    }
}