using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.Handlers.ShoppingListHandlers;
using ZeroWaste.Data.Services.ShoppingLists;
using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.Models;

namespace ZeroWaste.Controllers;

[Authorize]
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
        List<ShoppingList> shoppingLists = await _shoppingListHandler
            .GetShoppingListsByUserId(GetLoggedUser());
        return View(nameof(Index), shoppingLists);
    }
    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.Hidden = "hidden";
        var shoppingList = await _shoppingListHandler
            .GetShoppingListById(id);
        if (shoppingList is null)
            return View("NotFound");
        return View(nameof(Edit),shoppingList);
    }
    public async Task<IActionResult> ChangeIngredientSelection(int ingredientId, int shoppingListId)
    {
        //await _shoppingListHandler.HandleSelection(ingredientId);
        return RedirectToAction(nameof(Edit), new {id = shoppingListId});
    }
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _shoppingListHandler.IsShoppingListExists(id))
            return View("NotFound");
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
            return View("Create", shoppingListVm);
        var addedShoppingList = await _shoppingListHandler.Create(shoppingListVm, User.FindFirst(ClaimTypes.NameIdentifier).Value);
        return RedirectToAction("NewIngredientForShoppingList", "ShoppingListIngredients", new {id = addedShoppingList.Id});
    }
    [HttpPost]
    public async Task<IActionResult> EditTitle(ShoppingList shoppingList)
    {
        var tmp = await _shoppingListsService.IsShoppingListExists(shoppingList.Id);
        if (!(tmp))
            return View("NotFound");
        await _shoppingListsService.EditAsync(shoppingList);
        return RedirectToAction("Edit", new {id = shoppingList.Id});
    }

    private string GetLoggedUser()
    {
        var user = User.FindFirst(ClaimTypes.NameIdentifier);
        if (user is null)
            throw new Exception("There is no logged user");
        return user.Value;
    }
}