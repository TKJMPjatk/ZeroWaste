using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.Handlers.AutomatedShoppingList;
using ZeroWaste.Data.Services.RecipesSearch;
using ZeroWaste.Data.ViewModels.AutomatedShoppingList;

namespace ZeroWaste.Controllers;

public class AutomatedShoppingListController : Controller
{
    private readonly IAutomatedShoppingListHandler _automatedShoppingListHandler;
    public AutomatedShoppingListController(IAutomatedShoppingListHandler automatedShoppingListHandler)
    {
        _automatedShoppingListHandler = automatedShoppingListHandler;
    }
    public async Task<IActionResult> CreateFromRecipe(int recipeId)
    {
        var adddedItem = await _automatedShoppingListHandler.AddNewShoppingList(recipeId);
        AddedShoppingListVm addedShoppingListVm = new AddedShoppingListVm()
        {
            AddedShoppingListId = adddedItem.Id,
            RecipeId = recipeId
        };
        return View(addedShoppingListVm);
    }
}