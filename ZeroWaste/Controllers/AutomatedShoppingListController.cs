using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.Handlers.AutomatedShoppingList;
using ZeroWaste.Data.Services.RecipesSearch;
using ZeroWaste.Data.ViewModels.AutomatedShoppingList;

namespace ZeroWaste.Controllers;
[Authorize]
public class AutomatedShoppingListController : Controller
{
    private readonly IAutomatedShoppingListHandler _automatedShoppingListHandler;
    public AutomatedShoppingListController(IAutomatedShoppingListHandler automatedShoppingListHandler)
    {
        _automatedShoppingListHandler = automatedShoppingListHandler;
    }
    public async Task<IActionResult> CreateFromRecipe(int recipeId)
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var adddedItem = await _automatedShoppingListHandler.AddNewShoppingList(recipeId, userId);
        AddedShoppingListVm addedShoppingListVm = new AddedShoppingListVm()
        {
            AddedShoppingListId = adddedItem.Id,
            RecipeId = recipeId
        };
        return View(nameof(CreateFromRecipe), addedShoppingListVm);
    }
}