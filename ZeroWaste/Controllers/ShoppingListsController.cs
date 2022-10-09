using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.Handlers.ShoppingListHandlers;
using ZeroWaste.Data.Services.ShoppingLists;
using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.Models;

namespace ZeroWaste.Controllers;

public class ShoppingListsController : Controller
{
    private readonly IShoppingListsService _shoppingListsService;
    private readonly IShoppingListHandler _shoppingListHandler;
    public ShoppingListsController(IShoppingListsService shoppingListsService, IShoppingListHandler shoppingListHandler)
    {
        _shoppingListsService = shoppingListsService;
        _shoppingListHandler = shoppingListHandler;
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
    public async Task<IActionResult> ChangeIngredientSelection(int ingredientId, int shoppingListId)
    {
        await _shoppingListHandler.HandleSelection(ingredientId);
        return RedirectToAction(nameof(Edit), new {id = shoppingListId});
    }
    public async Task<IActionResult> Delete(int id)
    {
        await _shoppingListHandler.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> ConfirmShoppingList(int id)
    {
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(NewShoppingListVM shoppingListVm)
    {
        if (!(ModelState.IsValid))
            return View(shoppingListVm);
        var addedShoppingList = await _shoppingListHandler.Create(shoppingListVm);
        return RedirectToAction("NewIngredientForShoppingList", "ShoppingListIngredients", new {id = addedShoppingList.Id});
    }
}