using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.Enums;
using ZeroWaste.Data.ViewModels;

namespace ZeroWaste.Controllers;

public class AdminSearchRecipesController : Controller
{
    public IActionResult SearchForConfirm()
    {
        SearchRecipeResultsVm resultsVm = new SearchRecipeResultsVm()
        {
            SearchType = SearchType.Admin
        };
        return RedirectToAction("SearchResult", "SearchRecipes", resultsVm);
    }
}