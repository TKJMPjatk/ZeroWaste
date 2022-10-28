using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.Handlers.ShoppingListIngredients;
using ZeroWaste.Data.Helpers;
using ZeroWaste.Data.Services.ShoppingLists;
using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.Models;

namespace ZeroWaste.Controllers;

[Authorize]
public class ShoppingListIngredientsController : Controller
{
    private readonly IShoppingListIngredientsService _shoppingListIngredientService;
    private readonly IShoppingListIngredientsHandler _shoppingListIngredientsHandler;
    public ShoppingListIngredientsController(IShoppingListIngredientsService shoppingListIngredientService,  IShoppingListIngredientsHandler shoppingListIngredientsHandler)
    {
        _shoppingListIngredientService = shoppingListIngredientService;
        _shoppingListIngredientsHandler = shoppingListIngredientsHandler;
    }
    public async Task<IActionResult> NewIngredientForShoppingList(int id, string searchString)
    {
        var item = await _shoppingListIngredientsHandler
            .GetShoppingListIngredientsVm(id, searchString);
        return View(item);
    }
    public async Task<IActionResult> DeleteIngredientFromShoppingList(int shoppingListId, int ingredientId)
    {
        await _shoppingListIngredientService
            .DeleteIngredientFromShoppingList(shoppingListId, ingredientId);
        return RedirectToAction( "Edit", "ShoppingLists", new {id = shoppingListId});
    }
    public async Task<IActionResult> AddIngredientToShoppingList(int id, int shoppingListId)
    {
        await _shoppingListIngredientService
            .AddIngredientToShoppingList(shoppingListId, id);
        return RedirectToAction(nameof(NewIngredientForShoppingList), new {id = shoppingListId});
    }
    public async Task<IActionResult> EditQuantity(int shoppingListId)
    {
        var items = await _shoppingListIngredientService
            .GetNewIngredientsForShoppingList(shoppingListId);
        EditQuantityVM quantityVm = new EditQuantityVM()
        {
            ShoppingListId = shoppingListId,
            IngredientsToEditQuantity = items
        };
        return View(quantityVm);
    }
    [HttpPost]
    public async Task<IActionResult> EditQuantity(EditQuantityVM quantityVm)
    {
        await _shoppingListIngredientsHandler
            .EditQuantityOfNewIngredients(quantityVm);
        return RedirectToAction("Edit","ShoppingLists",new {id = quantityVm.ShoppingListId});
    }
}