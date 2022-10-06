using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.Helpers;
using ZeroWaste.Data.Services.ShoppingLists;
using ZeroWaste.Data.ViewModels.ShoppingList;

namespace ZeroWaste.Controllers;

public class ShoppingListIngredientController : Controller
{
    private readonly IShoppingListIngredientService _shoppingListIngredientService;
    private readonly IShoppingListIngredientsHelper _shoppingListIngredientsHelper;
    
    public ShoppingListIngredientController(IShoppingListIngredientService shoppingListIngredientService,  IShoppingListIngredientsHelper shoppingListIngredientsHelper)
    {
        _shoppingListIngredientService = shoppingListIngredientService;
        _shoppingListIngredientsHelper = shoppingListIngredientsHelper;
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
        return RedirectToAction(nameof(IngredientForShoppingList), new {id = shoppingListId});
    }
    /*public async Task<IActionResult> IngredientForShoppingList(int id)
    {
        var item = await _shoppingListIngredientsHelper
            .GetShoppingListIngredients(id);
        return View(item);
    }*/
    public async Task<IActionResult> IngredientForShoppingList(int id, string searchString)
    {
        ShoppingListIngredientsVM item;
        if (string.IsNullOrEmpty(searchString))
        {
            item = await _shoppingListIngredientsHelper
                .GetShoppingListIngredients(id);
        }
        else
        {
            item = await _shoppingListIngredientsHelper
                .GetShoppingListIngredients(id);
            item.IngredientShoppingListVms =
                item.IngredientShoppingListVms.Where(x => x.Name.Contains(searchString)).ToList();
        }
        return View(item);
    }
    public async Task<IActionResult> EditQuantity(int shoppingListId)
    {
        var items = await _shoppingListIngredientService
            .GetIngredientsForShoppingList(shoppingListId);
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
        return RedirectToAction("EditQuantity",1);
    }
}