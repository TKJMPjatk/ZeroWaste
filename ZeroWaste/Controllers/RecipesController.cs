using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZeroWaste.Data.Services.Recipes;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.Recipes;

namespace ZeroWaste.Controllers;

public class RecipesController : Controller
{
    private readonly IRecipesService _recipesService;
        
    public RecipesController(IRecipesService recipesService)
    {
        _recipesService = recipesService;
    }

    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind()] NewRecipeVM recipeVM)
    {
        if (!ModelState.IsValid)
        {
            return View(recipeVM);
        }

        await _recipesService.AddNewAsync(recipeVM);
        return RedirectToAction(nameof(AddIngredients));
    }
}