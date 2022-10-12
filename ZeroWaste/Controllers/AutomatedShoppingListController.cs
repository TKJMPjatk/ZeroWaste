using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.Handlers.AutomatedShoppingList;
using ZeroWaste.Data.Services.RecipesSearch;

namespace ZeroWaste.Controllers;

public class AutomatedShoppingListController
{
    private readonly IAutomatedShoppingListHandler _automatedShoppingListHandler;
    public AutomatedShoppingListController(IAutomatedShoppingListHandler automatedShoppingListHandler)
    {
        _automatedShoppingListHandler = automatedShoppingListHandler;
    }
    public async Task<IActionResult> CreateFromRecipe(int recipeId)
    {
        await _automatedShoppingListHandler.AddNewShoppingList(recipeId);
    }
}