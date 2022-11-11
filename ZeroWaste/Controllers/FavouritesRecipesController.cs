

using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.Services.FavouriteRecipes;

namespace ZeroWaste.Controllers;

public class FavouritesRecipesController : Controller
{
    private readonly IFavouritesRecipesService _favouritesRecipesService;
    public FavouritesRecipesController(IFavouritesRecipesService favouritesRecipesService)
    {
        _favouritesRecipesService = favouritesRecipesService;
    }
    [System.Web.Mvc.HttpPost]
    public async Task<IActionResult> UnmarkFavouriteRecipes(int recipeId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        await _favouritesRecipesService.DeleteFromFavourite(recipeId, userId);
        return RedirectToAction("SearchFavourite", "SearchRecipes");
    }
}