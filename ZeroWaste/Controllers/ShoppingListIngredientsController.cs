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
    private readonly IShoppingListIngredientsHandler _shoppingListIngredientsHandler;
    public ShoppingListIngredientsController(IShoppingListIngredientsHandler shoppingListIngredientsHandler)
    {
        _shoppingListIngredientsHandler = shoppingListIngredientsHandler;
    }
    public async Task<IActionResult> IngredientsToAdd(int id, string searchString, int ingredientTypeId)
    {
        var item = await _shoppingListIngredientsHandler.GetShoppingListIngredientsVm(id, searchString, ingredientTypeId);
        return View(nameof(IngredientsToAdd), item);
    }
    public async Task<IActionResult> ChangeCategory(int shoppingListId, int ingredientTypeId)
    {
        var item = await _shoppingListIngredientsHandler.GetShoppingListIngredientsVm(shoppingListId, null, ingredientTypeId);
        return View(nameof(IngredientsToAdd), item);
    }
    public async Task<IActionResult> DeleteShoppingListIngredient(int id)
    {
        int shoppingListId = await _shoppingListIngredientsHandler.HandleDeleteIngredientFromShoppingList(id);
        return RedirectToAction( "Edit", "ShoppingLists", new {id = shoppingListId});
    }
    public async Task<IActionResult> AddIngredientToShoppingList(int ingredientId, int shoppingListId, int typeId)
    {
        await _shoppingListIngredientsHandler
            .AddIngredientToShoppingList(ingredientId, shoppingListId);
        return RedirectToAction(nameof(IngredientsToAdd), new {id = shoppingListId, ingredientTypeId = typeId});
    }
    public async Task<IActionResult> DeleteIngredientFromShoppingList(int ingredientId, int shoppingListId, int typeId)
    {
        await _shoppingListIngredientsHandler
            .DeleteIngredientFromShoppingList(ingredientId, shoppingListId);
        return RedirectToAction(nameof(IngredientsToAdd), new {id = shoppingListId, ingredientTypeId = typeId});
    }
    public async Task<IActionResult> EditQuantity(int shoppingListId)
    {
        var model = await _shoppingListIngredientsHandler.GetEditQuantity(shoppingListId);
        return View(nameof(EditQuantity), model);
    }
    [HttpPost]
    public async Task<IActionResult> EditQuantity(EditQuantityVM quantityVm)
    {
        await _shoppingListIngredientsHandler
            .EditQuantityOfNewIngredients(quantityVm);
        return RedirectToAction("Edit","ShoppingLists",new {id = quantityVm.ShoppingListId});
    }
    public async Task<IActionResult> ChangeIngredientSelection(int shoppingListIngredientId)
    {
        int shoppingListId = await _shoppingListIngredientsHandler
                .ChangeShoppingListIngredientSelection(shoppingListIngredientId);
        return RedirectToAction("Edit", "ShoppingLists", new {id = shoppingListId});
    }
}