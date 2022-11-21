using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.Services.HatedRecipes;

namespace ZeroWaste.Controllers;

public class HatedRecipesController : Controller
{
    private readonly IHatedRecipesService _hatedRecipesService;
    public HatedRecipesController(IHatedRecipesService hatedRecipesService)
    {
        _hatedRecipesService = hatedRecipesService;
    }

    [HttpPost]
    public async Task<IActionResult> UnmarkHatedRecipes(int recipeId)
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        await _hatedRecipesService.DeleteHatedRecipes(recipeId, userId);
        return RedirectToAction("SearchHated", "SearchRecipes");
    }
}