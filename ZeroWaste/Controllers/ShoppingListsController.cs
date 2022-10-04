using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.Helpers;
using ZeroWaste.Data.Services;
using ZeroWaste.Models;

namespace ZeroWaste.Controllers;

public class ShoppingListsController : Controller
{
    private readonly IShoppingListsService _shoppingListsService;
    private readonly IIngredientsService _ingredientsService;
    private readonly IShoppingListIngredientsHelper _shoppingListIngredientsHelper;
    public ShoppingListsController(IShoppingListsService shoppingListsService, IIngredientsService ingredientsService, IShoppingListIngredientsHelper shoppingListIngredientsHelper)
    {
        _shoppingListsService = shoppingListsService;
        _ingredientsService = ingredientsService;
        _shoppingListIngredientsHelper = shoppingListIngredientsHelper;
    }
    public async Task<IActionResult> Index()
    {
        List<ShoppingList> shoppingLists = await _shoppingListsService.GetAllAsync();
        return View(shoppingLists);
    }
    public async Task<IActionResult> Edit(int id)
    {
        var shoppingList = await _shoppingListsService.GetByIdAsync(id);
        return View(shoppingList);
    }
    public async Task<IActionResult> Delete(int id)
    {
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> ConfirmShoppingList(int id)
    {
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> AddIngredientToShoppingList(int id)
    {
        var item = await _shoppingListIngredientsHelper.GetShoppingListIngredients(id);
        return View(item);
    }
}