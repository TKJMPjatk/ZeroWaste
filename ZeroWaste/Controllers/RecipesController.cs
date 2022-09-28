using Microsoft.AspNetCore.Mvc;
namespace ZeroWaste.Controllers;

public class RecipesController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult IngredientsSearch()
    {
        return View();
    }
}