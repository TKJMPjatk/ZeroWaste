using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.Handlers.ShoppingListHandlers;

namespace ZeroWaste.Controllers;

public class NavigationController : Controller
{
    private readonly IShoppingListHandler _shoppingListHandler;
    public NavigationController(IShoppingListHandler shoppingListHandler)
    {
        _shoppingListHandler = shoppingListHandler;
    }
    public async Task<IActionResult> NavigateAfterAddIngredientsToShoppingList(int shoppingListId)
    {
        if (await _shoppingListHandler.IsZeroQuantityIngredientsExists(shoppingListId))
        {
            return RedirectToAction("EditQuantity", "ShoppingListIngredients", new {shoppingListId = shoppingListId});
        }
        else
        {
            return RedirectToAction("Edit", "ShoppingLists", new {id = shoppingListId});
        }
    }
}