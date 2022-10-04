using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.Services;
using ZeroWaste.Models;

namespace ZeroWaste.Controllers;

public class ShoppingListsController : Controller
{
    private readonly IShoppingListsService _shoppingListsService;
    private readonly IIngredientsService _ingredientsService;
    public ShoppingListsController(IShoppingListsService shoppingListsService, IIngredientsService ingredientsService)
    {
        _shoppingListsService = shoppingListsService;
        _ingredientsService = ingredientsService;
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
        var ingredientList = await _ingredientsService.GetAllAsync();
        return View(ingredientList);
    }
}