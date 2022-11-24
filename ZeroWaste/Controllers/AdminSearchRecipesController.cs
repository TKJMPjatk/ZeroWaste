using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.Enums;
using ZeroWaste.Data.Handlers.AdminSearchHadlers;
using ZeroWaste.Data.ViewModels;

namespace ZeroWaste.Controllers;
public class AdminSearchRecipesController : Controller
{
    private readonly IAdminSearchHandler _adminSearchHandler; 
    public AdminSearchRecipesController(IAdminSearchHandler adminSearchHandler)
    {
        _adminSearchHandler = adminSearchHandler;
    }
    [Authorize(Roles = "Admin")]
    public IActionResult SearchForConfirm()
    {
        SearchRecipeResultsVm resultsVm = new SearchRecipeResultsVm()
        {
            SearchType = SearchType.Admin
        };
        return RedirectToAction("SearchForConfirm", "SearchRecipes", resultsVm);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> ConfirmRecipe(int recipeId)
    {
        SearchRecipeResultsVm resultsVm = await _adminSearchHandler
            .ConfirmRecipe(recipeId);
        return RedirectToAction("SearchForConfirm", "SearchRecipes");
    }
    [Authorize(Roles = "Admin")]
    [Microsoft.AspNetCore.Mvc.HttpPost]
    public async Task<IActionResult> RejectRecipe(int recipeId)
    {
        SearchRecipeResultsVm resultsVm = await _adminSearchHandler
            .RejectRecipe(recipeId);
        return RedirectToAction("SearchForConfirm", "SearchRecipes");
    }
}