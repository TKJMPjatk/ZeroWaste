using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.ViewModels.Recipes;

namespace ZeroWaste.Controllers;

public class RecipesController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> Create()
    {
        return View();
    }

    public async Task<IActionResult> Details(int id)
    {
        return View();
    }
}